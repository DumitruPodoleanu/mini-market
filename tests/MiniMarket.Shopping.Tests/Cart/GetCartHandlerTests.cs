using MiniMarket.Catalog.Contracts;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Features.Cart.GetCart;
using MiniMarket.Shopping.Tests.Fakes;

namespace MiniMarket.Shopping.Tests.Cart;

public sealed class GetCartHandlerTests
{
    private readonly FakeCartRepository _repository = new();
    private readonly FakeCurrentUser _user = new();

    private GetCartHandler Handler(params ProductSummary[] products) =>
        new(_repository, new FakeCatalogApi(products), _user);

    private void Add(ProductSummary product, int quantity) =>
        _repository.Items.Add(new CartItemEntity { Id = Guid.CreateVersion7(), UserId = _user.Id, ProductId = product.Id, Quantity = quantity });

    [Fact]
    public async Task HandleAsync_ReturnsLinesAndTotal()
    {
        var first = TestProducts.Published("first", price: 10.50m);
        var second = TestProducts.Published("second", price: 3m);
        Add(first, 2);
        Add(second, 3);

        var result = await Handler(first, second).HandleAsync(CancellationToken.None);

        var line = result.Value!.Items.Single(item => item.ProductSlug == "first");
        Assert.Equal(10.50m, line.Price);
        Assert.Equal(2, line.Quantity);
        Assert.Equal(21m, line.LineTotal);
        Assert.True(line.Available);
        Assert.Equal(30m, result.Value.Total);
    }

    [Fact]
    public async Task HandleAsync_LeavesUnpublishedItemOutOfTotal()
    {
        var available = TestProducts.Published("available", price: 10m);
        var unpublished = TestProducts.Unpublished();
        Add(available, 1);
        Add(unpublished, 1);

        var result = await Handler(available, unpublished).HandleAsync(CancellationToken.None);

        Assert.False(result.Value!.Items.Single(item => item.ProductSlug == unpublished.Slug).Available);
        Assert.Equal(10m, result.Value.Total);
    }

    [Fact]
    public async Task HandleAsync_LeavesItemOutOfTotal_WhenStockDroppedBelowQuantity()
    {
        var product = TestProducts.Published(price: 10m, stock: 1);
        Add(product, 2);

        var result = await Handler(product).HandleAsync(CancellationToken.None);

        Assert.False(Assert.Single(result.Value!.Items).Available);
        Assert.Equal(0m, result.Value.Total);
    }

    [Fact]
    public async Task HandleAsync_MarksItemAvailable_WhenStockEqualsQuantity()
    {
        var product = TestProducts.Published(stock: 2);
        Add(product, 2);

        var result = await Handler(product).HandleAsync(CancellationToken.None);

        Assert.True(Assert.Single(result.Value!.Items).Available);
    }

    [Fact]
    public async Task HandleAsync_HidesProductThatNoLongerExists()
    {
        Add(TestProducts.Published("deleted"), 1);

        var result = await Handler().HandleAsync(CancellationToken.None);

        Assert.Empty(result.Value!.Items);
        Assert.Equal(0m, result.Value.Total);
    }

    [Fact]
    public async Task HandleAsync_ReturnsEmptyCart_WhenNothingWasAdded()
    {
        var result = await Handler().HandleAsync(CancellationToken.None);

        Assert.Empty(result.Value!.Items);
        Assert.Equal(0m, result.Value.Total);
    }

    [Fact]
    public async Task HandleAsync_DoesNotReturnItemsOfAnotherUser()
    {
        var product = TestProducts.Published();
        _repository.Items.Add(new CartItemEntity { Id = Guid.CreateVersion7(), UserId = Guid.NewGuid(), ProductId = product.Id, Quantity = 1 });

        var result = await Handler(product).HandleAsync(CancellationToken.None);

        Assert.Empty(result.Value!.Items);
    }
}
