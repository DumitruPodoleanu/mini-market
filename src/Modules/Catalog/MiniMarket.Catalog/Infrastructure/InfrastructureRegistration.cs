using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;
using MiniMarket.Catalog.Infrastructure.Repositories;
using MiniMarket.Catalog.Infrastructure.Seed;

namespace MiniMarket.Catalog.Infrastructure;

internal static class InfrastructureRegistration
{
    public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services, IConfiguration configuration)
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
            options.UseSeeding((context, _) => CatalogSeed.Seed(context));
            options.UseAsyncSeeding((context, _, cancellationToken) => CatalogSeed.SeedAsync(context, cancellationToken));
        });

        services.AddScoped<ICatalogDbContext>(serviceProvider => serviceProvider.GetRequiredService<CatalogDbContext>());

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISlugRepository, SlugRepository>();

        services.AddExceptionHandler<UniqueViolationExceptionHandler>();

        return services;
    }
}