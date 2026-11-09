using Microsoft.Extensions.Logging.Abstractions;
using MiniMarket.Catalog.Contracts;
using MiniMarket.Shared.Results;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Features.Cart.SetCartItem;
using MiniMarket.Shopping.Tests.Fakes;

namespace MiniMarket.Shopping.Tests.Cart;

public sealed class SetCartItemHandlerTests
{
    private readonly FakeCartRepository _repository = new();
    private readonly FakeCurrentUser _user = new();

    private SetCartItemHandler Handler(params ProductSummary[] products) =>
        new(_repository, TestProducts.Lookup(new FakeCatalogApi(products)), _user, NullLogger<SetCartItemHandler>.Instance);

    private static SetCartItemCommand Command(string slug, int quantity) => new(slug, new SetCartItemRequest(quantity));

    [Fact]
    public async Task HandleAsync_AddsNewItem()
    {
        var product = TestProducts.Published();

        var result = await Handler(product).HandleAsync(Command(product.Slug, 2), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var item = Assert.Single(_repository.Items);
        Assert.Equal(_user.Id, item.UserId);
        Assert.Equal(product.Id, item.ProductId);
        Assert.Equal(2, item.Quantity);
    }

    [Fact]
    public async Task HandleAsync_ReplacesQuantity_WhenItemExists()
    {
        var product = TestProducts.Published();
        var handler = Handler(product);

        await handler.HandleAsync(Command(product.Slug, 2), CancellationToken.None);
        await handler.HandleAsync(Command(product.Slug, 4), CancellationToken.None);

        Assert.Equal(4, Assert.Single(_repository.Items).Quantity);
    }

    [Fact]
    public async Task HandleAsync_AcceptsQuantityEqualToStock()
    {
        var product = TestProducts.Published(stock: 3);

        var result = await Handler(product).HandleAsync(Command(product.Slug, 3), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleAsync_ReturnsConflict_WhenQuantityIsAboveStock()
    {
        var product = TestProducts.Published(stock: 3);

        var result = await Handler(product).HandleAsync(Command(product.Slug, 4), CancellationToken.None);

        Assert.Equal(ErrorType.Conflict, result.Error!.Type);
        Assert.Empty(_repository.Items);
    }

    [Fact]
    public async Task HandleAsync_KeepsOldQuantity_WhenNewQuantityIsAboveStock()
    {
        var product = TestProducts.Published(stock: 3);
        _repository.Items.Add(new CartItemEntity { Id = Guid.CreateVersion7(), UserId = _user.Id, ProductId = product.Id, Quantity = 2 });

        await Handler(product).HandleAsync(Command(product.Slug, 4), CancellationToken.None);

        Assert.Equal(2, Assert.Single(_repository.Items).Quantity);
    }

    [Fact]
    public async Task HandleAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var result = await Handler().HandleAsync(Command("missing", 1), CancellationToken.None);

        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
    }

    [Fact]
    public async Task HandleAsync_ReturnsConflict_WhenProductIsUnpublished()
    {
        var product = TestProducts.Unpublished();

        var result = await Handler(product).HandleAsync(Command(product.Slug, 1), CancellationToken.None);

        Assert.Equal(ErrorType.Conflict, result.Error!.Type);
        Assert.Empty(_repository.Items);
    }
}
