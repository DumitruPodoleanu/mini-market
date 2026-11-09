using System.ComponentModel.DataAnnotations;

namespace MiniMarket.Shopping.Features.Common;

internal sealed class ShoppingOptions
{
    public const string SectionName = "Shopping";

    [Range(1, 1000)]
    public int MaxWishlistPageSize { get; set; }

    [Range(1, 1000)]
    public int MaxCartItemQuantity { get; set; }
}