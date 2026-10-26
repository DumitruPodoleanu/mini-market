using MiniMarket.Api.ErrorHandling;
using MiniMarket.Api.Middleware;
using MiniMarket.Catalog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddCatalogModule(builder.Configuration);

var app = builder.Build();

await app.MigrateCatalogDatabaseAsync();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseExceptionHandler();

app.MapCatalogEndpoints();

app.Run();
