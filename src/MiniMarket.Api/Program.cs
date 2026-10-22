using MiniMarket.Catalog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCatalogModule(builder.Configuration);

var app = builder.Build();

app.MapCatalogEndpoints();

app.Run();