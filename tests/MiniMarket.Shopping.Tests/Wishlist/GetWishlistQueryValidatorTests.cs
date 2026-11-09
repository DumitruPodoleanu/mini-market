using Microsoft.Extensions.Options;
using MiniMarket.Shopping.Features.Common;
using MiniMarket.Shopping.Features.Wishlist.GetWishlist;

namespace MiniMarket.Shopping.Tests.Wishlist;

public sealed class GetWishlistQueryValidatorTests
{
    private readonly GetWishlistQueryValidator _validator =
        new(Options.Create(new ShoppingOptions { MaxWishlistPageSize = 100 }));

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 100)]
    [InlineData(500, 20)]
    [InlineData(21474836, 100)]
    public void Validate_Accepts_ValuesInRange(int page, int pageSize) =>
        Assert.True(_validator.Validate(new GetWishlistQuery(page, pageSize)).IsValid);

    [Theory]
    [InlineData(0, 20, "Page must be at least 1.")]
    [InlineData(-1, 20, "Page must be at least 1.")]
    [InlineData(1, 0, "Page size must be between 1 and 100.")]
    [InlineData(1, 101, "Page size must be between 1 and 100.")]
    [InlineData(21474837, 100, "Page cannot exceed 21474836.")]
    [InlineData(int.MaxValue, 100, "Page cannot exceed 21474836.")]
    public void Validate_Rejects_ValuesOutOfRange(int page, int pageSize, string message)
    {
        var result = _validator.Validate(new GetWishlistQuery(page, pageSize));

        Assert.Equal(message, Assert.Single(result.Errors).ErrorMessage);
    }
}
