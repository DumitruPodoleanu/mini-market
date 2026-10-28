using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.GetProduct;

internal sealed class GetProductHandler(IProductRepository productRepository)
{
    public async Task<Result<ProductResponse>> HandleAsync(Guid id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null)
            return ProductErrors.NotFound(id);

        return product.ToResponse();
    }
}