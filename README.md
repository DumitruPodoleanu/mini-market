# MiniMarket

A modular monolith on .NET 10 Minimal APIs. Right now it has one module, **Catalog** (products sold by vendors, with brands and categories), backed by PostgreSQL through EF Core.

## Run it

Requirements: .NET 10 SDK and Docker.

```bash
docker compose up -d db
```

```bash
dotnet run --project src/MiniMarket.Api
```

- The API listens on `http://localhost:8080`.
- At startup it applies the migrations and, on an empty database, seeds 3 vendors, 4 brands, 5 categories and 13 products, with Russian names for the categories and some products.
- API docs (Development only): `http://localhost:8080/scalar`

To start from an empty database again:

```bash
docker compose down -v
```

To run the API in Docker as well:

```bash
docker compose up --build
```

## Tests

```bash
dotnet test
```

Unit tests cover slug generation, slug resolution and the translation fallback.

## Try the API

- `src/MiniMarket.Api/MiniMarket.Api.http` has working requests: list with filters, get in Romanian and Russian, upsert (create and update), delete. Run it top to bottom on a freshly seeded database; the get and delete requests reuse the slug of the product the create request makes.
- `src/MiniMarket.Api/MiniMarket.Api.BadRequests.http` has one request per error case, each with the expected status and message.

## Endpoints

| Method | Route | |
|---|---|---|
| GET | `/api/products` | paged list, see filters below |
| GET | `/api/products/{slug}` | one product |
| PATCH | `/api/products?vendorSlug=&sku=` | upsert: creates the product, or updates the fields that were sent |
| DELETE | `/api/products/{slug}` | deletes the product and its slug |

List query parameters, all optional: `categorySlug`, `brandSlug`, `name` (contains, case-insensitive, also matches translated names), `published`, `minPrice`, `maxPrice`, `sort` (`nameAsc`, `priceAsc`, `priceDesc`), `page` (from 1), `pageSize` (1 to 100, default 20).

## Response shape

Every endpoint except DELETE answers with the same envelope:

```json
{ "success": true,  "data": { }, "errors": [] }
{ "success": false, "data": null, "errors": ["Product with slug 'x' was not found."] }
```

Responses never expose database ids; products, brands, categories and vendors are identified by their slug.

| Status | When |
|---|---|
| 200 | get, list, or upsert that updated an existing product |
| 201 | upsert that created a product; `Location: /api/products/{slug}` |
| 204 | product deleted |
| 400 | validation failed, a new product is missing required fields, an unknown brand or category, an invalid slug |
| 404 | product not found, or the vendor in the upsert address does not exist |
| 409 | the requested slug is taken, or a concurrent request created the same product or slug |

## Upsert

`PATCH /api/products?vendorSlug=istore&sku=MTJV3`

- A product is identified by its **vendor and SKU**: the SKU comes from the vendor's own system, so it is only unique per vendor. The database enforces this with a unique index on `(VendorId, Sku)`.
- If the product does not exist, it is created. `name`, `price`, `brandSlug` and `categorySlugs` are required; `published` defaults to false and `stockQuantity` to 0.
- If it exists, only the fields present in the body change. Sending `{ "price": 4699 }` updates the price and nothing else.
- Vendors are never created by this endpoint. An unknown vendor is a 404.
- SKUs are trimmed and uppercased before the lookup, so `" mtjv3 "` finds `MTJV3`.

## Slugs

- Every product, brand, category and vendor has a slug, stored in one shared `Slugs` table with a unique index, so a slug is unique across all of them (a brand and a vendor both named Samsung get `samsung` and `samsung-2`).
- When no slug is sent, it is generated from the name: transliterated to ASCII (Romanian and Cyrillic), lowercased, words joined by `-`, at most 150 characters. If it is taken, `-2`, `-3`, … is appended.
- A slug sent in the upsert body is normalized the same way, must share at least one word with the name, and returns 409 if another entity already uses it.

## Translations

- The language comes from the `Accept-Language` header. Supported: `ro` (default) and `ru`. Anything else falls back to `ro`.
- `Name` on the entity is the Romanian value. Translations live in their own tables (`ProductTranslations`, `CategoryTranslations`), one row per entity and language, with a cascade foreign key.
- Each translated field falls back to the Romanian value when the translation is missing.
- Adding a translatable field is one nullable property on the translation entity, one line of configuration and a migration. Enabling translations for another entity is the same: brands and vendors don't have them because their names are proper nouns.
- Translations are read-only through the API for now; they come from the seed.

## Structure

```
src/
  MiniMarket.Api/              host: Program.cs, middleware, appsettings, .http files
  Shared/MiniMarket.Shared/    Result, Error, ApiResponse, PagedResponse (no ASP.NET dependency)
  Modules/Catalog/MiniMarket.Catalog/
    Domain/
      Entities/                Product, Brand, Category, Vendor
        Common/                base classes: BaseEntity, SluggableEntity, TranslatableEntity, TranslationEntity
        Slugs/                 SlugEntity, SlugEntityType
        Translations/          ProductTranslationEntity, CategoryTranslationEntity
      Repositories/            repository interfaces, ProductFilter, ProductSort
      Constraints/             length limits and supported languages
    Features/
      Products/<Action>/       one folder per use case: Endpoint, Handler, Request, Validator
      Common/                  shared by several features: contracts, mappings, errors,
                               Result → HTTP, validation filter, slugs, localization
    Infrastructure/            DbContext, EF configurations, repositories, migrations, seed
tests/
  MiniMarket.Catalog.Tests/    unit tests
```

## How a request flows

1. **Validation filter:** the FluentValidation validator for the request runs first. Invalid input returns 400 before any handler code runs.
2. **Handler:** does the work and the checks that need the database (vendor, brand and categories exist, slug available). It returns a `Result` and never throws for expected situations.
3. **Endpoint:** turns the `Result` into the status code and the envelope (`ResultExtensions`).

Endpoints are found automatically: every class implementing `IEndpoint` is registered at startup. A unique-index violation from a concurrent request is turned into a 409 by an exception handler in the infrastructure layer.

## Configuration

Database settings live in the `CatalogDatabase` section, bound to `CatalogDatabaseOptions` and validated at startup:

| Key | Where |
|---|---|
| `ConnectionString` | `appsettings.Development.json` locally, `CatalogDatabase__ConnectionString` in Docker |
| `CommandTimeoutSeconds`, `MaxRetryCount` | `appsettings.json` |
