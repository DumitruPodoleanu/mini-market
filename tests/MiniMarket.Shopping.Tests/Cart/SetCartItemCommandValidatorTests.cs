using Microsoft.Extensions.Options;
using MiniMarket.Shopping.Features.Cart.SetCartItem;
using MiniMarket.Shopping.Features.Common;

namespace MiniMarket.Shopping.Tests.Cart;

public sealed class SetCartItemCommandValidatorTests
{
    private readonly SetCartItemCommandValidator _validator =
        new(Options.Create(new ShoppingOptions { MaxCartItemQuantity = 99 }));

    private static SetCartItemCommand Command(int? quantity) => new("slug", new SetCartItemRequest(quantity));

    [Theory]
    [InlineData(1)]
    [InlineData(99)]
    public void Validate_Accepts_QuantityInRange(int quantity) =>
        Assert.True(_validator.Validate(Command(quantity)).IsValid);

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100)]
    public void Validate_Rejects_QuantityOutOfRange(int quantity)
    {
        var result = _validator.Validate(Command(quantity));

        Assert.Equal("Quantity must be between 1 and 99.", Assert.Single(result.Errors).ErrorMessage);
    }

    [Fact]
    public void Validate_Rejects_MissingBody()
    {
        var result = _validator.Validate(new SetCartItemCommand("slug", null!));

        Assert.Equal("Request body is required.", Assert.Single(result.Errors).ErrorMessage);
    }

    [Fact]
    public void Validate_Rejects_MissingQuantity()
    {
        var result = _validator.Validate(Command(null));

        Assert.Equal("Quantity is required.", Assert.Single(result.Errors).ErrorMessage);
    }
}
