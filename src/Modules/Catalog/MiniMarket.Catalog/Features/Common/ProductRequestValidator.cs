using System.Text.RegularExpressions;
using FluentValidation;
using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Features.Common;

internal abstract class ProductRequestValidator<T> : AbstractValidator<T> where T : IProductRequest
{
    protected ProductRequestValidator()
    {
        RuleFor(p => p.Sku)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("SKU is required.")
            .Must(sku => sku.Trim().Length <= ProductConstraints.SkuMaxLength)
            .WithMessage($"SKU cannot exceed {ProductConstraints.SkuMaxLength} characters.")
            .Must(sku => Regex.IsMatch(sku.Trim(), "^[A-Za-z0-9]+(-[A-Za-z0-9]+)*$"))
            .WithMessage("SKU can only contain letters, digits and dashes.");

        RuleFor(p => p.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Must(name => name.Trim().Length <= ProductConstraints.NameMaxLength)
            .WithMessage($"Name cannot exceed {ProductConstraints.NameMaxLength} characters.");

        RuleFor(p => p.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");

        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity cannot be negative.");

        RuleFor(p => p.BrandId)
            .NotEmpty()
            .WithMessage("Brand ID is required.");

        RuleFor(p => p.CategoryIds).NotEmpty().WithMessage("At least one category ID is required.");
        RuleForEach(p => p.CategoryIds).NotEmpty().WithMessage("Category IDs cannot be empty.");
    }
}