using MiniMarket.Api.Middleware;
using MiniMarket.Catalog;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.Configure<RouteHandlerOptions>(options => options.ThrowOnBadRequest = false);
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<UniqueViolationExceptionHandler>();

builder.Services.AddCatalogModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.MigrateCatalogDatabaseAsync();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseExceptionHandler();
app.UseRequestLocalization();

app.MapEndpoints();

app.Run();
