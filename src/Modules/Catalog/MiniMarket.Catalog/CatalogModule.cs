using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.CreateProduct;
using MiniMarket.Catalog.Features.DeleteProduct;
using MiniMarket.Catalog.Features.GetProduct;
using MiniMarket.Catalog.Features.ListProducts;
using MiniMarket.Catalog.Features.UpdateProduct;
using MiniMarket.Catalog.Infrastructure;

namespace MiniMarket.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<CatalogDatabaseOptions>()
            .Bind(configuration.GetSection(CatalogDatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<CatalogDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<CatalogDatabaseOptions>>().Value;
            options.UseNpgsql(databaseOptions.ConnectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", CatalogDbContext.Schema);
                npgsql.CommandTimeout(databaseOptions.CommandTimeoutSeconds);
                npgsql.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
            });
        });

        services.AddScoped<ICatalogDbContext>(serviceProvider => serviceProvider.GetRequiredService<CatalogDbContext>());

        services.AddValidatorsFromAssembly(typeof(CatalogModule).Assembly, includeInternalTypes: true);
        services.AddEndpoints(typeof(CatalogModule).Assembly);

        services.AddScoped<IProductRepository, EfProductRepository>();
        services.AddScoped<IBrandRepository, EfBrandRepository>();
        services.AddScoped<ICategoryRepository, EfCategoryRepository>();
        services.AddScoped<ProductReferences>();

        services.AddScoped<GetProductHandler>();
        services.AddScoped<ListProductsHandler>();
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<DeleteProductHandler>();

        return services;
    }

    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapEndpoints();
        return app;
    }

    public static async Task MigrateCatalogDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
