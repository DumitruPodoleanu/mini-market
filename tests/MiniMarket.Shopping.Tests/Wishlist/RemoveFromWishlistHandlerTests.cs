using Microsoft.Extensions.Logging.Abstractions;
using MiniMarket.Catalog.Contracts;
using MiniMarket.Shared.Results;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Features.Wishlist.RemoveFromWishlist;
using MiniMarket.Shopping.Tests.Fakes;

namespace MiniMarket.Shopping.Tests.Wishlist;

public sealed class RemoveFromWishlistHandlerTests
{
    private readonly FakeWishlistRepository _repository = new();
    private readonly FakeCurrentUser _user = new();

    private RemoveFromWishlistHandler Handler(params ProductSummary[] products) =>
        new(_repository, TestProducts.Lookup(new FakeCatalogApi(products)), _user, NullLogger<RemoveFromWishlistHandler>.Instance);

    private void Save(ProductSummary product) =>
        _repository.Items.Add(new WishlistItemEntity { Id = Guid.CreateVersion7(), UserId = _user.Id, ProductId = product.Id });

    [Fact]
    public async Task HandleAsync_RemovesSavedProduct()
    {
        var product = TestProducts.Published();
        Save(product);

        var result = await Handler(product).HandleAsync(product.Slug, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(_repository.Items);
    }

    [Fact]
    public async Task HandleAsync_RemovesProductThatWasUnpublishedAfterSaving()
    {
        var product = TestProducts.Unpublished();
        Save(product);

        var result = await Handler(product).HandleAsync(product.Slug, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(_repository.Items);
    }

    [Fact]
    public async Task HandleAsync_ReturnsNotFound_WhenProductIsNotSaved()
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

    [Fact]
    public async Task HandleAsync_DoesNotRemoveItemOfAnotherUser()
    {
        var product = TestProducts.Published();
        _repository.Items.Add(new WishlistItemEntity { Id = Guid.CreateVersion7(), UserId = Guid.NewGuid(), ProductId = product.Id });

        var result = await Handler(product).HandleAsync(product.Slug, CancellationToken.None);

        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
        Assert.Single(_repository.Items);
    }
}
