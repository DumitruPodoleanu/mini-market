using Microsoft.Extensions.Logging.Abstractions;
using MiniMarket.Catalog.Contracts;
using MiniMarket.Shared.Results;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Features.Cart.RemoveCartItem;
using MiniMarket.Shopping.Tests.Fakes;

namespace MiniMarket.Shopping.Tests.Cart;

public sealed class RemoveCartItemHandlerTests
{
    private readonly FakeCartRepository _repository = new();
    private readonly FakeCurrentUser _user = new();

    private RemoveCartItemHandler Handler(params ProductSummary[] products) =>
        new(_repository, TestProducts.Lookup(new FakeCatalogApi(products)), _user, NullLogger<RemoveCartItemHandler>.Instance);

    private void Add(ProductSummary product) =>
        _repository.Items.Add(new CartItemEntity { Id = Guid.CreateVersion7(), UserId = _user.Id, ProductId = product.Id, Quantity = 1 });

    [Fact]
    public async Task HandleAsync_RemovesItem()
    {
        var product = TestProducts.Published();
        Add(product);

        var result = await Handler(product).HandleAsync(product.Slug, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(_repository.Items);
    }

    [Fact]
    public async Task HandleAsync_RemovesProductThatWasUnpublishedAfterAdding()
    {
        var product = TestProducts.Unpublished();
        Add(product);

        var result = await Handler(product).HandleAsync(product.Slug, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(_repository.Items);
    }

    [Fact]
    public async Task HandleAsync_ReturnsNotFound_WhenItemIsNotInCart()
    {
        var product = TestProducts.Published();

        var result = await Handler(product).HandleAsync(product.Slug, CancellationToken.None);

        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
    }

    [Fact]
    public async Task HandleAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var result = await Handler().HandleAsync("missing", CancellationToken.None);

        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
    }
}
