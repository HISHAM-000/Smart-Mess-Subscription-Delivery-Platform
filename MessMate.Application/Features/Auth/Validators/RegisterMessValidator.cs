using FluentValidation;
using MessMate.Application.Features.Auth.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Validators
{
    public class RegisterMessValidator:AbstractValidator<RegisterMessCommand>
    {
        public RegisterMessValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required")
            .Must(e => !string.IsNullOrWhiteSpace(e) && !e.Trim().Any(char.IsWhiteSpace))
            .WithMessage("Email must not contain spaces in between")
            .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character")
            .Must(e => !string.IsNullOrWhiteSpace(e) && !e.Trim().Any(char.IsWhiteSpace))
            .WithMessage("Password must not contain spaces in between");

            RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^[0-9]{10}$").WithMessage("Invalid phone number");

            //RuleFor(x => x.MessName)
            //.NotEmpty().WithMessage("MessName is required");

            RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("LicenseNumber is required")
            .Must(e => !string.IsNullOrWhiteSpace(e) && !e.Trim().Any(char.IsWhiteSpace))
            .WithMessage("License number must not contain spaces in between");
        }
    }
}
