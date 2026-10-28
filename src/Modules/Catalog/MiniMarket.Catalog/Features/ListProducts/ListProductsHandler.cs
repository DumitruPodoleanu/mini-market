using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.ListProducts;

internal sealed class ListProductsHandler(IProductRepository productRepository)
{
    public async Task<Result<IReadOnlyList<ProductResponse>>> HandleAsync(string? categorySlug)
    {
        var products = await productRepository.GetAllAsync(categorySlug);

        return Result<IReadOnlyList<ProductResponse>>.Success(products.Select(p => p.ToResponse()).ToList());
    }
}