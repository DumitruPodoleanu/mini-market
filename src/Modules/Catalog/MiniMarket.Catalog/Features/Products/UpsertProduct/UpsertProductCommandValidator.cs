using System.Text.RegularExpressions;
using FluentValidation;
using MiniMarket.Catalog.Domain.Constraints;

namespace MiniMarket.Catalog.Features.Products.UpsertProduct;

internal sealed partial class UpsertProductCommandValidator : AbstractValidator<UpsertProductCommand>
{
    public UpsertProductCommandValidator()
    {
        RuleFor(c => c.VendorSlug)
            .NotEmpty()
            .WithMessage("Vendor slug is required.");

        RuleFor(c => c.Sku)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("SKU is required.")
            .Must(sku => sku!.Trim().Length <= ProductConstraints.SkuMaxLength)
            .WithMessage($"SKU cannot exceed {ProductConstraints.SkuMaxLength} characters.")
            .Must(sku => SkuFormat().IsMatch(sku!.Trim()))
            .WithMessage("SKU can only contain letters, digits and dashes.");

        RuleFor(c => c.Body)
            .NotNull()
            .WithMessage("Request body is required.");

        When(c => c.Body is not null, () =>
        {
            RuleFor(c => c.Body.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .MaximumLength(ProductConstraints.NameMaxLength)
                .WithMessage($"Name cannot exceed {ProductConstraints.NameMaxLength} characters.")
                .When(c => c.Body.Name is not null);

            RuleFor(c => c.Body.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(c => c.Body.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock quantity cannot be negative.");

            RuleFor(c => c.Body.BrandSlug)
                .NotEmpty()
                .WithMessage("Brand slug cannot be empty.")
                .When(c => c.Body.BrandSlug is not null);

            RuleFor(c => c.Body.CategorySlugs)
                .NotEmpty()
                .WithMessage("At least one category is required.")
                .When(c => c.Body.CategorySlugs is not null);

            RuleForEach(c => c.Body.CategorySlugs)
                .NotEmpty()
                .WithMessage("Category slugs cannot be empty.");
        });
    }

    [GeneratedRegex("^[A-Za-z0-9]+(-[A-Za-z0-9]+)*$")]
    private static partial Regex SkuFormat();
}
