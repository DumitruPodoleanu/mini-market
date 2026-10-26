using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.ListProducts;

internal sealed class ListProductsHandler(IProductRepository productRepository)
{
    public async Task<IReadOnlyList<ProductResponse>> HandleAsync(string? categorySlug)
    {
        var products = await productRepository.GetAllAsync(categorySlug);

        return products.Select(ProductResponse.FromDomain).ToList();
    }
}
