using FluentValidation;
using MessMate.Application.Features.Delivery.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Delivery.Validators
{
    public class AssignDeliveryCommandValidator : AbstractValidator<AssignDeliveryCommand>
    {
        public AssignDeliveryCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Invalid order id.");
            RuleFor(x => x.StaffId)
                .GreaterThan(0).WithMessage("Invalid staff id.");
        }
    }
}
