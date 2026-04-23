using FluentValidation;
using MessMate.Application.Features.Delivery.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Delivery.Validators
{
    public class ConfirmDeliveryCommandValidator : AbstractValidator<ConfirmDeliveryCommand>
    {
        public ConfirmDeliveryCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Invalid order id.");
            RuleFor(x => x.OTP)
                .NotEmpty().WithMessage("OTP is required.")
                .Length(6).WithMessage("OTP must be 6 digits.")
                .Matches(@"^\d{6}$").WithMessage("OTP must be numeric.");
        }
    }
}
