using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Features.GetProduct;

internal sealed record ProductResponse(Guid Id, string Name, decimal Price, int DiscountPercentage, decimal FinalPrice)
{
    public static ProductResponse FromDomain(Product product) =>
        new(product.Id, product.Name, product.Price, product.DiscountPercentage, product.FinalPrice);
}