using FluentValidation;
using MessMate.Application.Features.Applications.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Validators
{
    public class ResubmitOwnerVallidator:AbstractValidator<ResubmitOwnerCommand>
    {
        public ResubmitOwnerVallidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required")
            .Must(e => !string.IsNullOrWhiteSpace(e) && !e.Trim().Any(char.IsWhiteSpace))
            .WithMessage("Email must not contain spaces in between")
            .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^[0-9]{10}$").WithMessage("Invalid phone number");

            RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("LicenseNumber is required")
            .Must(e => !string.IsNullOrWhiteSpace(e) && !e.Trim().Any(char.IsWhiteSpace))
            .WithMessage("License number must not contain spaces in between");
        }
    }
}
