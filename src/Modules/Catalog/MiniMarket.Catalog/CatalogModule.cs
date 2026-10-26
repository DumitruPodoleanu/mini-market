using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        var connectionString = configuration.GetConnectionString("Catalog")
            ?? throw new InvalidOperationException("Connection string 'Catalog' is missing.");

        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", CatalogDbContext.Schema)));

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
        var products = app.MapGroup("/api/products");
        GetProductEndpoint.Map(products);
        ListProductsEndpoint.Map(products);
        CreateProductEndpoint.Map(products);
        UpdateProductEndpoint.Map(products);
        DeleteProductEndpoint.Map(products);
        return app;
    }

    public static async Task MigrateCatalogDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
