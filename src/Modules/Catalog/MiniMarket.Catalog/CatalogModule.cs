using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniMarket.Catalog.Features;
using MiniMarket.Catalog.Features.Common.Endpoints;
using MiniMarket.Catalog.Infrastructure;
using MiniMarket.Catalog.Infrastructure.Persistence;

namespace MiniMarket.Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddCatalogInfrastructure(configuration)
            .AddCatalogServices()
            .AddCatalogPresentation();

    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapEndpoints();
        return app;
    }

    public static Task MigrateCatalogDatabaseAsync(this WebApplication app) =>
        app.Services.MigrateCatalogDatabaseAsync();
}