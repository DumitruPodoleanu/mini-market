using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MiniMarket.Shopping.Infrastructure.Persistence;
using MiniMarket.Infrastructure.Persistence;

namespace MiniMarket.Shopping.Infrastructure;

internal static class InfrastructureRegistration
{
    public static IServiceCollection AddShoppingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ShoppingDatabaseOptions>()
            .Bind(configuration.GetSection(ShoppingDatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<ShoppingDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<ShoppingDatabaseOptions>>().Value;
            options.UseModuleDatabase(databaseOptions, ShoppingDbContext.Schema);
        });

        services.AddScoped<IShoppingDbContext>(serviceProvider => serviceProvider.GetRequiredService<ShoppingDbContext>());

        return services;
    }
}