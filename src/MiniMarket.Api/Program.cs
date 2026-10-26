using MiniMarket.Catalog;
using MiniMarket.Api.ErrorHandling;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.MapCatalogEndpoints();

app.Run();