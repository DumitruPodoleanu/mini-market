using FluentValidation;
using Microsoft.Extensions.Options;
using MiniMarket.Shopping.Features.Common;

namespace MiniMarket.Shopping.Features.Cart.SetCartItem;

internal sealed class SetCartItemCommandValidator : AbstractValidator<SetCartItemCommand>
{
    public SetCartItemCommandValidator(IOptions<ShoppingOptions> options)
    {
        var maxQuantity = options.Value.MaxCartItemQuantity;

        RuleFor(c => c.Body)
            .NotNull()
            .WithMessage("Request body is required.");

        When(c => c.Body is not null, () =>
        {
            RuleFor(c => c.Body.Quantity)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Quantity is required.")
                .InclusiveBetween(1, maxQuantity)
                .WithMessage($"Quantity must be between 1 and {maxQuantity}.");
        });
    }
}
