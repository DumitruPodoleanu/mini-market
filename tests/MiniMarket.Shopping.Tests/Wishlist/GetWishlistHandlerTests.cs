using MiniMarket.Catalog.Contracts;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Features.Wishlist.GetWishlist;
using MiniMarket.Shopping.Tests.Fakes;

namespace MiniMarket.Shopping.Tests.Wishlist;

public sealed class GetWishlistHandlerTests
{
    private readonly FakeWishlistRepository _repository = new();
    private readonly FakeCurrentUser _user = new();

    private GetWishlistHandler Handler(params ProductSummary[] products) =>
        new(_repository, new FakeCatalogApi(products), _user);

    private void Save(ProductSummary product) =>
        _repository.Items.Add(new WishlistItemEntity { Id = Guid.CreateVersion7(), UserId = _user.Id, ProductId = product.Id });

    [Fact]
    public async Task HandleAsync_ReturnsLiveProductData()
    {
        var product = TestProducts.Published(price: 42m);
        Save(product);

        var result = await Handler(product).HandleAsync(new GetWishlistQuery(), CancellationToken.None);

        var item = Assert.Single(result.Value!.Items);
        Assert.Equal(product.Slug, item.ProductSlug);
        Assert.Equal(product.Name, item.Name);
        Assert.Equal(42m, item.Price);
        Assert.True(item.Available);
    }

    [Fact]
    public async Task HandleAsync_MarksUnpublishedProductAsUnavailable()
    {
        var product = TestProducts.Unpublished();
        Save(product);

        var result = await Handler(product).HandleAsync(new GetWishlistQuery(), CancellationToken.None);

        Assert.False(Assert.Single(result.Value!.Items).Available);
    }

    [Fact]
    public async Task HandleAsync_MarksProductWithoutStockAsUnavailable()
    {
        var product = TestProducts.Published(stock: 0);
        Save(product);

        var result = await Handler(product).HandleAsync(new GetWishlistQuery(), CancellationToken.None);

        Assert.False(Assert.Single(result.Value!.Items).Available);
    }

    [Fact]
    public async Task HandleAsync_HidesProductThatNoLongerExists()
    {
        var product = TestProducts.Published();
        Save(product);
        Save(TestProducts.Published("deleted"));

        var result = await Handler(product).HandleAsync(new GetWishlistQuery(), CancellationToken.None);

        Assert.Single(result.Value!.Items);
    }

    [Fact]
    public async Task HandleAsync_ReturnsPagingValues()
    {
        var products = Enumerable.Range(1, 5).Select(number => TestProducts.Published($"product-{number}")).ToArray();
        foreach (var product in products)
            Save(product);

        var result = await Handler(products).HandleAsync(new GetWishlistQuery(Page: 2, PageSize: 2), CancellationToken.None);

        Assert.Equal(2, result.Value!.Items.Count);
        Assert.Equal(2, result.Value.Page);
        Assert.Equal(2, result.Value.PageSize);
        Assert.Equal(5, result.Value.TotalItems);
        Assert.Equal(3, result.Value.TotalPages);
    }

    [Fact]
    public async Task HandleAsync_ReturnsEmptyPage_WhenNothingIsSaved()
    {
        var result = await Handler().HandleAsync(new GetWishlistQuery(), CancellationToken.None);

        Assert.Empty(result.Value!.Items);
        Assert.Equal(0, result.Value.TotalItems);
        Assert.Equal(0, result.Value.TotalPages);
    }

    [Fact]
    public async Task HandleAsync_DoesNotReturnItemsOfAnotherUser()
    {
        var product = TestProducts.Published();
        _repository.Items.Add(new WishlistItemEntity { Id = Guid.CreateVersion7(), UserId = Guid.NewGuid(), ProductId = product.Id });

        var result = await Handler(product).HandleAsync(new GetWishlistQuery(), CancellationToken.None);

        Assert.Empty(result.Value!.Items);
    }
}
