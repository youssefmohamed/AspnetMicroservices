using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .NotEmpty()
                .WithMessage("{UserName} can not be empty");

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage("{EmailAddress} can not be empty");

            RuleFor(x => x.TotalPrice)
                .GreaterThan(0)
                .WithMessage("{TotalPrice} must be greater than 0");
        }
    }
}
