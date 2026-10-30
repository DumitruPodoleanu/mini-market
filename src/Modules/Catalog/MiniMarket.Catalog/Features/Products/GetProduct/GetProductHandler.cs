using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.GetProduct;

internal sealed class GetProductHandler(IProductRepository productRepository)
{
    public async Task<Result<ProductResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return ProductErrors.NotFound(id);

        return product.ToResponse();
    }
}