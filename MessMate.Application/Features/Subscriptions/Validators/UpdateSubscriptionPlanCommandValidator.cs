using FluentValidation;
using MessMate.Application.Features.Subscriptions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Validators
{
    public class UpdateSubscriptionPlanCommandValidator : AbstractValidator<UpdateSubscriptionPlanCommand>
    {
        public UpdateSubscriptionPlanCommandValidator()
        {
            RuleFor(x => x.PlanId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.PlanType).IsInEnum();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.MinActiveDays).GreaterThan(0);
            RuleFor(x => x).Must(x => x.IsBreakfast || x.IsLunch || x.IsDinner)
                .WithMessage("At least one meal slot must be selected.");
        }
    }
}
