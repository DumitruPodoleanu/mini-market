using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniMarket.Infrastructure.Persistence;
using MiniMarket.Shopping.Features;
using MiniMarket.Shopping.Infrastructure;
using MiniMarket.Shopping.Infrastructure.Persistence;

namespace MiniMarket.Shopping;

public static class ShoppingModule
{
    public static IServiceCollection AddShoppingModule(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddShoppingInfrastructure(configuration)
            .AddShoppingServices()
            .AddShoppingPresentation();

    public static Task MigrateShoppingDatabaseAsync(this WebApplication app) =>
        app.Services.MigrateDatabaseAsync<ShoppingDbContext>();
}