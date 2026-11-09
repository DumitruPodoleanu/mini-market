using MiniMarket.Catalog.Contracts;
using MiniMarket.Shared.Results;
using MiniMarket.Shared.Users;
using MiniMarket.Shopping.Domain.Repositories;
using MiniMarket.Shopping.Features.Common.Mappings;
using MiniMarket.Shopping.Features.Common.Responses;

namespace MiniMarket.Shopping.Features.Cart.GetCart;

internal sealed class GetCartHandler(
    ICartRepository cartRepository,
    ICatalogApi catalogApi,
    ICurrentUser currentUser)
{
    public async Task<Result<CartResponse>> HandleAsync(CancellationToken cancellationToken)
    {
        var items = await cartRepository.GetAllAsync(currentUser.Id, cancellationToken);

        var productIds = items.Select(item => item.ProductId).ToList();
        var products = await catalogApi.GetProductsByIdsAsync(productIds, cancellationToken);
        var productsById = products.ToDictionary(product => product.Id);

        var responses = items
            .Where(item => productsById.ContainsKey(item.ProductId))
            .Select(item => item.ToResponse(productsById[item.ProductId]))
            .ToList();

        var total = responses.Where(item => item.Available).Sum(item => item.LineTotal);

        return new CartResponse(responses, total);
    }
}
