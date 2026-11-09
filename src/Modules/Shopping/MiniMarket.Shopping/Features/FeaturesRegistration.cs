using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Shopping.Features.Cart.ClearCart;
using MiniMarket.Shopping.Features.Cart.GetCart;
using MiniMarket.Shopping.Features.Cart.RemoveCartItem;
using MiniMarket.Shopping.Features.Cart.SetCartItem;
using MiniMarket.Shopping.Features.Common;
using MiniMarket.Shopping.Features.Wishlist.AddToWishlist;
using MiniMarket.Shopping.Features.Wishlist.GetWishlist;
using MiniMarket.Shopping.Features.Wishlist.RemoveFromWishlist;

namespace MiniMarket.Shopping.Features;

internal static class FeaturesRegistration
{
    public static IServiceCollection AddShoppingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ShoppingOptions>()
            .Bind(configuration.GetSection(ShoppingOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<ProductLookup>();
        services.AddScoped<AddToWishlistHandler>();
        services.AddScoped<RemoveFromWishlistHandler>();
        services.AddScoped<GetWishlistHandler>();
        services.AddScoped<SetCartItemHandler>();
        services.AddScoped<RemoveCartItemHandler>();
        services.AddScoped<ClearCartHandler>();
        services.AddScoped<GetCartHandler>();

        return services;
    }

    public static IServiceCollection AddShoppingPresentation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ShoppingModule).Assembly, includeInternalTypes: true);
        services.AddEndpoints(typeof(ShoppingModule).Assembly);
        return services;
    }
}
