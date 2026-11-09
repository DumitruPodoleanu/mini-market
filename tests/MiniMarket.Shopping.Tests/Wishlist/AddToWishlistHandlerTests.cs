using Microsoft.Extensions.Logging.Abstractions;
using MiniMarket.Catalog.Contracts;
using MiniMarket.Shared.Results;
using MiniMarket.Shopping.Features.Wishlist.AddToWishlist;
using MiniMarket.Shopping.Tests.Fakes;

namespace MiniMarket.Shopping.Tests.Wishlist;

public sealed class AddToWishlistHandlerTests
{
    private readonly FakeWishlistRepository _repository = new();
    private readonly FakeCurrentUser _user = new();

    private AddToWishlistHandler Handler(params ProductSummary[] products) =>
        new(_repository, TestProducts.Lookup(new FakeCatalogApi(products)), _user, NullLogger<AddToWishlistHandler>.Instance);

    [Fact]
    public async Task HandleAsync_SavesProductForCurrentUser()
    {
        var product = TestProducts.Published();

        var result = await Handler(product).HandleAsync(product.Slug, CancellationToken.None);

        Assert.True(result.IsSuccess);
        var item = Assert.Single(_repository.Items);
        Assert.Equal(_user.Id, item.UserId);
        Assert.Equal(product.Id, item.ProductId);
    }

    [Fact]
    public async Task HandleAsync_DoesNotAddTwice_WhenRepeated()
    {
        var product = TestProducts.Published();
        var handler = Handler(product);

        await handler.HandleAsync(product.Slug, CancellationToken.None);
        var result = await handler.HandleAsync(product.Slug, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(_repository.Items);
    }

    [Fact]
    public async Task HandleAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var result = await Handler().HandleAsync("missing", CancellationToken.None);

        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
        Assert.Empty(_repository.Items);
    }

    [Fact]
    public async Task HandleAsync_ReturnsConflict_WhenProductIsUnpublished()
    {
        var product = TestProducts.Unpublished();

        var result = await Handler(product).HandleAsync(product.Slug, CancellationToken.None);

        Assert.Equal(ErrorType.Conflict, result.Error!.Type);
        Assert.Empty(_repository.Items);
    }
}
