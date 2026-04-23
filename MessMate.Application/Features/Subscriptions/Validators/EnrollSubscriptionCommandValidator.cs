using FluentValidation;
using MessMate.Application.Features.Subscriptions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Validators
{
    public class EnrollSubscriptionCommandValidator : AbstractValidator<EnrollSubscriptionCommand>
    {
        public EnrollSubscriptionCommandValidator()
        {
            RuleFor(x => x.PlanId).GreaterThan(0);
            RuleFor(x => x.MessId).GreaterThan(0);
            RuleFor(x => x.DeliveryAddress).NotEmpty().MaximumLength(300);
            RuleFor(x => x.DeliveryLat).InclusiveBetween(-90, 90);
            RuleFor(x => x.DeliveryLng).InclusiveBetween(-180, 180);
            RuleFor(x => x.PaymentMethod).IsInEnum();
        }
    }
}
