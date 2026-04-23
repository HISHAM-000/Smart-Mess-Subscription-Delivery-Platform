using FluentValidation;
using MessMate.Application.Features.Orders.Commands;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Orders.Validators
{
    public class UpdateOrderStatusCommandValidator: AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
               .GreaterThan(0).WithMessage("Invalid order id.");

            RuleFor(x => x.NewStatus)
                .Must(s =>
                    s == OrderStatus.Preparing ||
                    s == OrderStatus.OutForDelivery ||
                    s == OrderStatus.Delivered)
                .WithMessage(
                    "Status can only be updated to " +
                    "Preparing, OutForDelivery or Delivered.");
        }
    }
    
}
