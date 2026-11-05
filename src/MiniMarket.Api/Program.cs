using MiniMarket.Api.Middleware;
using MiniMarket.Api.Users;
using MiniMarket.Catalog;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Persistence;
using MiniMarket.Shared.Users;
using MiniMarket.Shopping;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.Configure<RouteHandlerOptions>(options => options.ThrowOnBadRequest = false);
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<UniqueViolationExceptionHandler>();

builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddShoppingModule(builder.Configuration);
builder.Services.AddSingleton<ICurrentUser, GuestCurrentUser>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.MigrateCatalogDatabaseAsync();
await app.MigrateShoppingDatabaseAsync();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseExceptionHandler();
app.UseRequestLocalization();

app.MapEndpoints();

app.Run();
