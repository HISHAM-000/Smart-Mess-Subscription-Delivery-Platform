using FluentValidation;
using MessMate.Application.Features.Applications.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Validators
{
    public class RejectMessCommandValidator:AbstractValidator<RejectMessCommand >
    {
        public RejectMessCommandValidator()
        {
            RuleFor(x => x.MessId)
               .GreaterThan(0).WithMessage("Invalid mess id.");
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Rejection reason is required.")
                .MaximumLength(500);
        }
    }
}
