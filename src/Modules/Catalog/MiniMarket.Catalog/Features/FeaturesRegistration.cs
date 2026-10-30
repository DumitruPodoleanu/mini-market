using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Endpoints;
using MiniMarket.Catalog.Features.Products.CreateProduct;
using MiniMarket.Catalog.Features.Products.DeleteProduct;
using MiniMarket.Catalog.Features.Products.GetProduct;
using MiniMarket.Catalog.Features.Products.ListProducts;
using MiniMarket.Catalog.Features.Products.UpdateProduct;

namespace MiniMarket.Catalog.Features;

internal static class FeaturesRegistration
{
    public static IServiceCollection AddCatalogServices(this IServiceCollection services)
    {
        services.AddScoped<ProductReferences>();

        services.AddScoped<GetProductHandler>();
        services.AddScoped<ListProductsHandler>();
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<DeleteProductHandler>();

        return services;
    }

    public static IServiceCollection AddCatalogPresentation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CatalogModule).Assembly, includeInternalTypes: true);
        services.AddEndpoints(typeof(CatalogModule).Assembly);

        return services;
    }
}