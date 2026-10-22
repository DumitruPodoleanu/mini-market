using MiniMarket.Catalog.Domain;
using MiniMarket.Shared;

namespace MiniMarket.Catalog.Features.GetProduct;

internal sealed class GetProductHandler(IProductRepository productRepository)
{
    public async Task<ProductResponse> HandleAsync(Guid id)
    {
        var product = await productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Product {id} was not found.");

        return ProductResponse.FromDomain(product);
    }
}