using MiniMarket.Catalog.Contracts;
using MiniMarket.Shared.Results;
using MiniMarket.Shared.Users;
using MiniMarket.Shopping.Domain.Repositories;
using MiniMarket.Shopping.Features.Common.Mappings;
using MiniMarket.Shopping.Features.Common.Responses;

namespace MiniMarket.Shopping.Features.Wishlist.GetWishlist;

internal sealed class GetWishlistHandler(
    IWishlistRepository wishlistRepository,
    ICatalogApi catalogApi,
    ICurrentUser currentUser)
{
    public async Task<Result<PagedResponse<WishlistItemResponse>>> HandleAsync(GetWishlistQuery query, CancellationToken cancellationToken)
    {
        var (items, totalItems) = await wishlistRepository.GetPageAsync(currentUser.Id, query.Page, query.PageSize, cancellationToken);

        var productIds = items.Select(item => item.ProductId).ToList();
        var products = await catalogApi.GetProductsByIdsAsync(productIds, cancellationToken);
        var productsById = products.ToDictionary(product => product.Id);

        var responses = items
            .Where(item => productsById.ContainsKey(item.ProductId))
            .Select(item => item.ToResponse(productsById[item.ProductId]))
            .ToList();

        var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

        return new PagedResponse<WishlistItemResponse>(responses, query.Page, query.PageSize, totalItems, totalPages);
    }
}
