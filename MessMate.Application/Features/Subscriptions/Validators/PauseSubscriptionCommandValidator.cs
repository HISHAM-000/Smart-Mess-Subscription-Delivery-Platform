using FluentValidation;
using MessMate.Application.Features.Subscriptions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Validators
{
    public class PauseSubscriptionCommandValidator : AbstractValidator<PauseSubscriptionCommand>
    {
        public PauseSubscriptionCommandValidator()
        {
            RuleFor(x => x.SubscriptionId).GreaterThan(0);

            RuleFor(x => x.PauseFrom)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.Date))
                .WithMessage("Pause date cannot be in the past.");

            RuleFor(x => x.PauseUntil)
                .GreaterThan(x => x.PauseFrom)
                .WithMessage("PauseUntil must be after PauseFrom.");
        }
    }
}
