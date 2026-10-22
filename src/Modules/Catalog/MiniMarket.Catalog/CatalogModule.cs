using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Infrastructure;
using MiniMarket.Catalog.Features.GetProduct;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MiniMarket.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        services.AddScoped<GetProductHandler>();
        return services;
    }

    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var products = app.MapGroup("/api/products");
        GetProductEndpoint.Map(products);
        return app;
    }
}