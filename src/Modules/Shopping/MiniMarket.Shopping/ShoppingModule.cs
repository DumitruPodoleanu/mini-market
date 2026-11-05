using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniMarket.Shopping.Features;
using MiniMarket.Shopping.Infrastructure;

namespace MiniMarket.Shopping;

public static class ShoppingModule
{
    public static IServiceCollection AddShoppingModule(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddShoppingInfrastructure(configuration)
            .AddShoppingServices()
            .AddShoppingPresentation();
}