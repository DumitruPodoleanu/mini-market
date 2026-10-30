using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.ListProducts;

internal sealed class ListProductsHandler(IProductRepository productRepository)
{
    public async Task<Result<IReadOnlyList<ProductResponse>>> HandleAsync(string? categorySlug, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync(categorySlug, cancellationToken);

        return Result<IReadOnlyList<ProductResponse>>.Success(products.Select(p => p.ToResponse()).ToList());
    }
}