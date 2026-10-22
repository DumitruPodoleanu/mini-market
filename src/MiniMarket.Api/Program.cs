using MiniMarket.Catalog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCatalogModule();

var app = builder.Build();

app.MapCatalogEndpoints();

app.Run();