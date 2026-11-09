using FluentValidation;
using Microsoft.Extensions.Options;
using MiniMarket.Shopping.Features.Common;

namespace MiniMarket.Shopping.Features.Wishlist.GetWishlist;

internal sealed class GetWishlistQueryValidator : AbstractValidator<GetWishlistQuery>
{
    public GetWishlistQueryValidator(IOptions<ShoppingOptions> options)
    {
        var maxPageSize = options.Value.MaxWishlistPageSize;

        var maxPage = int.MaxValue / maxPageSize;

        RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.")
            .LessThanOrEqualTo(maxPage)
            .WithMessage($"Page cannot exceed {maxPage}.");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, maxPageSize)
            .WithMessage($"Page size must be between 1 and {maxPageSize}.");
    }
}
