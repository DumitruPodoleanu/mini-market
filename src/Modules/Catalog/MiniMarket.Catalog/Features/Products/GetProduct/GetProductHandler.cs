using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Responses;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Catalog.Features.Common.Localization;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.GetProduct;

internal sealed class GetProductHandler(IProductRepository productRepository)
{
    public async Task<Result<ProductResponse>> HandleAsync(string slug, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetBySlugAsync(slug, cancellationToken);

        if (product is null)
            return ProductErrors.NotFound(slug);

        return product.ToResponse(RequestLanguage.Current);
    }
}