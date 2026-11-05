using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniMarket.Catalog.Features;
using MiniMarket.Catalog.Infrastructure;
using MiniMarket.Catalog.Infrastructure.Persistence;
using MiniMarket.Infrastructure.Persistence;

namespace MiniMarket.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddCatalogInfrastructure(configuration)
            .AddCatalogServices()
            .AddCatalogPresentation();

    public static Task MigrateCatalogDatabaseAsync(this WebApplication app) =>
        app.Services.MigrateDatabaseAsync<CatalogDbContext>();
}