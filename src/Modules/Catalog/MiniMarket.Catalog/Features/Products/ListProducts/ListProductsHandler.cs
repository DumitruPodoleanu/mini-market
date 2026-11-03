using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Catalog.Features.Common.Localization;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.ListProducts;

internal sealed class ListProductsHandler(IProductRepository productRepository)
{
    public async Task<Result<PagedResponse<ProductResponse>>> HandleAsync(ListProductsQuery query, CancellationToken cancellationToken)
    {
        var sort = Enum.TryParse<ProductSort>(query.Sort, ignoreCase: true, out var parsed) ? parsed : ProductSort.NameAsc;

        var filter = new ProductFilter(query.CategorySlug, query.Name, query.Published, query.MinPrice, query.MaxPrice, query.BrandSlug, sort, query.Page, query.PageSize);
        var (items, totalItems) = await productRepository.GetPageAsync(filter, cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);
        var language = RequestLanguage.Current;

        return new PagedResponse<ProductResponse>(
            items.Select(p => p.ToResponse(language)).ToList(),
            query.Page,
            query.PageSize,
            totalItems,
            totalPages);
    }
}