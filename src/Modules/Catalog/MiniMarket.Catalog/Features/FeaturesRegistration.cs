using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Endpoints;
using MiniMarket.Catalog.Features.Common.Slugs;
using MiniMarket.Catalog.Features.Products.DeleteProduct;
using MiniMarket.Catalog.Features.Products.GetProduct;
using MiniMarket.Catalog.Features.Products.ListProducts;
using MiniMarket.Catalog.Features.Products.UpsertProduct;

namespace MiniMarket.Catalog.Features;

internal static class FeaturesRegistration
{
    public static IServiceCollection AddCatalogServices(this IServiceCollection services)
    {
        services.AddScoped<ProductReferences>();
        services.AddScoped<SlugService>();

        services.AddScoped<GetProductHandler>();
        services.AddScoped<ListProductsHandler>();
        services.AddScoped<DeleteProductHandler>();
        services.AddScoped<UpsertProductHandler>();

        return services;
    }

    public static IServiceCollection AddCatalogPresentation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CatalogModule).Assembly, includeInternalTypes: true);
        services.AddEndpoints(typeof(CatalogModule).Assembly);

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.SetDefaultCulture(LanguageConstraints.Default)
                .AddSupportedCultures(LanguageConstraints.Supported)
                .AddSupportedUICultures(LanguageConstraints.Supported);
            options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];
        });

        return services;
    }
}