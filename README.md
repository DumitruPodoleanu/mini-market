# MiniMarket

A modular monolith on .NET 10 Minimal APIs. It has two modules, backed by PostgreSQL through EF Core:

- **Catalog:** products sold by vendors, with brands and categories.
- **Shopping:** the wishlist and the cart of the current user.

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

Unit tests cover slug generation, slug resolution, the translation fallback, and the Shopping handlers and validators. The Shopping tests replace the database and the Catalog module with fakes.

## Try the API

- `src/MiniMarket.Api/MiniMarket.Api.http` has working requests: list with filters, get in Romanian and Russian, upsert (create and update), delete, wishlist and cart. Run it top to bottom on a freshly seeded database; the get and delete requests reuse the slug of the product the create request makes.
- `src/MiniMarket.Api/MiniMarket.Api.BadRequests.http` has one request per error case, each with the expected status and message.

## Endpoints

| Method | Route | |
|---|---|---|
| GET | `/api/products` | paged list, see filters below |
| GET | `/api/products/{slug}` | one product |
| PATCH | `/api/products?vendorSlug=&sku=` | upsert: creates the product, or updates the fields that were sent |
| DELETE | `/api/products/{slug}` | deletes the product and its slug |
| GET | `/api/wishlist?page=&pageSize=` | paged wishlist, newest first; `page` from 1, `pageSize` 1 to 100, default 20 |
| PUT | `/api/wishlist/{productSlug}` | saves a product; repeating it changes nothing |
| DELETE | `/api/wishlist/{productSlug}` | removes a product from the wishlist |
| GET | `/api/cart` | cart lines and total |
| PUT | `/api/cart/items/{productSlug}` | sets the quantity, body `{ "quantity": 2 }`; adds the line if it is new |
| DELETE | `/api/cart/items/{productSlug}` | removes a line |
| DELETE | `/api/cart` | empties the cart; repeating it changes nothing |

List query parameters, all optional: `categorySlug`, `brandSlug`, `name` (contains, case-insensitive, also matches translated names), `published`, `minPrice`, `maxPrice`, `sort` (`nameAsc`, `priceAsc`, `priceDesc`), `page` (from 1), `pageSize` (1 to 100, default 20).

## Response shape

Every endpoint that returns content or an error answers with the same envelope:

```json
{ "success": true,  "data": { }, "errors": [] }
{ "success": false, "data": null, "errors": ["Product with slug 'x' was not found."] }
```

Responses never expose database ids; products, brands, categories and vendors are identified by their slug.

| Status | When |
|---|---|
| 200 | get, list, or upsert that updated an existing product |
| 201 | upsert that created a product; `Location: /api/products/{slug}` |
| 204 | product deleted, wishlist or cart changed |
| 400 | validation failed, a new product is missing required fields, an unknown brand or category, an invalid slug |
| 404 | product not found, the vendor in the upsert address does not exist, or the product is not in the wishlist or cart |
| 409 | the requested slug is taken, a concurrent request created the same row, a product added to the wishlist or cart is unpublished, or a cart quantity is above the stock |

## Upsert

`PATCH /api/products?vendorSlug=istore&sku=MTJV3`

- A product is identified by its **vendor and SKU**: the SKU comes from the vendor's own system, so it is only unique per vendor. The database enforces this with a unique index on `(VendorId, Sku)`.
- If the product does not exist, it is created. `name`, `price`, `brandSlug` and `categorySlugs` are required; `published` defaults to false and `stockQuantity` to 0.
- If it exists, only the fields present in the body change. Sending `{ "price": 4699 }` updates the price and nothing else.
- Vendors are never created by this endpoint. An unknown vendor is a 404.
- SKUs are trimmed and uppercased before the lookup, so `" mtjv3 "` finds `MTJV3`.

## Wishlist and cart

- There is no login yet. `ICurrentUser` gives handlers the user, and `GuestCurrentUser` always returns the same hardcoded id, so all callers share one wishlist and one cart. Real users only need a new `ICurrentUser` implementation.
- Shopping never reads Catalog's tables. It calls `ICatalogApi` from `MiniMarket.Catalog.Contracts` and stores only the product id, in its own `shopping` schema with no foreign key to Catalog.
- Names and prices are read live from Catalog, so they are always current and follow `Accept-Language`.
- `available` tells the client whether the product can be bought now. A wishlist item is available when it is published and in stock. A cart line is available when it is published and the stock covers its quantity. The cart `total` only counts available lines.
- A product that was unpublished after being saved stays listed as unavailable and can still be removed. Saving a new unpublished product is a 409.
- Removes and quantity updates are single SQL statements, so simultaneous requests cannot fail halfway: one remove returns 204 and the others 404.

Known limitations:

- A product deleted from Catalog is hidden from the wishlist and cart, but its row stays. The wishlist `totalItems` still counts it, and removing it by slug returns 404 because the slug is gone. `DELETE /api/cart` does clear it. The proper fix is for Catalog to notify Shopping when a product is deleted.
- Identical `PUT` requests for a new item sent at the same moment can get a 409 from the unique index. The data stays correct, with one row, and repeating the request returns 204.

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
  Shared/
    MiniMarket.Shared/         Result, Error, ApiResponse, PagedResponse, base entity, ICurrentUser (no ASP.NET dependency)
    MiniMarket.Infrastructure/ endpoint scan, Result → HTTP, validation filter, repository base, database setup
  Modules/Catalog/MiniMarket.Catalog.Contracts/
                               ICatalogApi and ProductSummary, all that other modules may see of Catalog
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
      Common/                  shared by several features: responses, mappings, errors, slugs, localization
    Infrastructure/            DbContext, EF configurations, repositories, migrations, seed
  Modules/Shopping/MiniMarket.Shopping/
    Domain/                    WishlistItemEntity, CartItemEntity, repository interfaces
    Features/
      Wishlist/<Action>/       one folder per use case
      Cart/<Action>/           one folder per use case
      Common/                  ProductLookup, ShoppingErrors, ShoppingOptions
    Infrastructure/            DbContext, EF configurations, repositories, migrations
tests/
  MiniMarket.Catalog.Tests/    unit tests
  MiniMarket.Shopping.Tests/   unit tests with fake repositories and a fake ICatalogApi
```

## How a request flows

1. **Validation filter:** the FluentValidation validator for the request runs first. Invalid input returns 400 before any handler code runs.
2. **Handler:** does the work and the checks that need the database (vendor, brand and categories exist, slug available). It returns a `Result` and never throws for expected situations.
3. **Endpoint:** turns the `Result` into the status code and the envelope (`ResultExtensions`).

Endpoints are found automatically: every class implementing `IEndpoint` is registered at startup. A unique-index violation from a concurrent request is turned into a 409 by an exception handler in the infrastructure layer.

## Configuration

Each module has its own database section, `CatalogDatabase` and `ShoppingDatabase`, bound to its options class and validated at startup:

| Key | Where |
|---|---|
| `ConnectionString` | `appsettings.Development.json` locally, `CatalogDatabase__ConnectionString` and `ShoppingDatabase__ConnectionString` in Docker |
| `CommandTimeoutSeconds`, `MaxRetryCount` | `appsettings.json` |

Shopping limits live in the `Shopping` section of `appsettings.json`, bound to `ShoppingOptions`. The app refuses to start if a value is outside 1 to 1000:

| Key | Default | |
|---|---|---|
| `MaxWishlistPageSize` | 100 | largest `pageSize` on the wishlist |
| `MaxCartItemQuantity` | 99 | largest quantity of one product in the cart |
