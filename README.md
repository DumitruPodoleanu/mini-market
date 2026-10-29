# MiniMarket

A modular monolith on .NET 10 Minimal APIs. Right now it has one module, **Catalog** (products, brands, categories), backed by PostgreSQL through EF Core.

## Run it

Requirements: .NET 10 SDK and Docker.

```bash
docker compose up -d db
```

```bash
dotnet run --project src/MiniMarket.Api
```

- The API listens on `http://localhost:8080`.
- At startup it applies the migrations and seeds 3 brands, 4 categories and 4 products.
- API docs (Development only): `http://localhost:8080/scalar`

To start from an empty database again:

```bash
docker compose down -v
```

To run the API in Docker as well:

```bash
docker compose up --build
```

## Try the API

- `src/MiniMarket.Api/MiniMarket.Api.http` has working requests: list, get, create, update, delete. Send the create request first; the requests after it reuse its id.
- `src/MiniMarket.Api/MiniMarket.Api.BadRequests.http` has one request per error case, each with the expected status and message.

## Response shape

Every endpoint answers with the same envelope:

```json
{ "success": true,  "data": { }, "errors": [] }
{ "success": false, "data": null, "errors": ["Product … was not found."] }
```

| Status | When |
|---|---|
| 200 | success (get, list, update, delete) |
| 201 | product created; `data` is the new id |
| 400 | validation failed, or an unknown brand or category |
| 404 | product not found |
| 409 | SKU already used by another product |

## Endpoints

| Method | Route | |
|---|---|---|
| GET | `/api/products?categorySlug=` | list, optionally filtered by category |
| GET | `/api/products/{id}` | one product |
| POST | `/api/products` | create |
| PUT | `/api/products/{id}` | update |
| DELETE | `/api/products/{id}` | delete |

## Structure

```
src/
  MiniMarket.Api/              host: Program.cs, middleware, appsettings, .http files
  Shared/MiniMarket.Shared/    Result, Error, ApiResponse (no ASP.NET dependency)
  Modules/Catalog/MiniMarket.Catalog/
    Domain/                    entities (plain data), repository interfaces, length constraints
    Features/<Action>/         one folder per use case: Endpoint, Handler, Request, Validator
    Features/Common/           shared by several features: validation filter, mappings,
                               errors, Result → HTTP, endpoint discovery
    Infrastructure/            DbContext, EF configurations, repositories, migrations, seed
```

## How a request flows

1. **Validation filter:** the FluentValidation validator for the request runs first. Invalid input returns 400 before any handler code runs.
2. **Handler:** does the work and the checks that need the database (brand exists, SKU unique). It returns a `Result` and never throws.
3. **Endpoint:** turns the `Result` into the status code and the envelope (`ResultExtensions`).

Endpoints are found automatically: every class implementing `IEndpoint` is registered at startup.

## Configuration

Database settings live in the `CatalogDatabase` section, bound to `CatalogDatabaseOptions` and validated at startup:

| Key | Where |
|---|---|
| `ConnectionString` | `appsettings.Development.json` locally, `CatalogDatabase__ConnectionString` in Docker |
| `CommandTimeoutSeconds`, `MaxRetryCount` | `appsettings.json` |
