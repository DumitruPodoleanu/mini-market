using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MiniMarket.Infrastructure.Endpoints;

namespace MiniMarket.Shopping.Features;

internal static class FeaturesRegistration
{
    public static IServiceCollection AddShoppingServices(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddShoppingPresentation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ShoppingModule).Assembly, includeInternalTypes: true);
        services.AddEndpoints(typeof(ShoppingModule).Assembly);
        return services;
    }
}