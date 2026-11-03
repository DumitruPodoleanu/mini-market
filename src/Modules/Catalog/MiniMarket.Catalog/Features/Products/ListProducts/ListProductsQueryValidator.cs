using FluentValidation;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Repositories;

namespace MiniMarket.Catalog.Features.Products.ListProducts;

internal sealed class ListProductsQueryValidator : AbstractValidator<ListProductsQuery>
{
    public ListProductsQueryValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, ListProductsQuery.MaxPageSize)
            .WithMessage($"Page size must be between 1 and {ListProductsQuery.MaxPageSize}.");

        RuleFor(q => q.MinPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum price cannot be negative.");

        RuleFor(q => q.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maximum price cannot be negative.");

        RuleFor(q => q)
            .Must(q => q.MinPrice is null || q.MaxPrice is null || q.MinPrice <= q.MaxPrice)
            .WithMessage("Minimum price cannot be greater than maximum price.");

        RuleFor(q => q.Name)
            .MaximumLength(ProductConstraints.NameMaxLength)
            .WithMessage($"Name filter cannot exceed {ProductConstraints.NameMaxLength} characters.");

        RuleFor(q => q.Sort)
            .Must(BeValidSort)
            .WithMessage("Sort must be one of: nameAsc, priceAsc, priceDesc.");
    }

    private static bool BeValidSort(string? sort) =>
        sort is null
        || (!int.TryParse(sort, out _) && Enum.TryParse<ProductSort>(sort, ignoreCase: true, out _));
}