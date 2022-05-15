using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
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
