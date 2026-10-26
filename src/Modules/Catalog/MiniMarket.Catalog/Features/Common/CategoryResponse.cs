using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Features.Common;

internal sealed record CategoryResponse(
    Guid Id,
    string Name,
    string Slug
)
{
    public static CategoryResponse FromDomain(Category category) =>
        new(
            category.Id,
            category.Name,
            category.Slug
            );
}
