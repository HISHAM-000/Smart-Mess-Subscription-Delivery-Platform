using FluentValidation;
using MessMate.Application.Features.MessStaff.Commands;

namespace MessMate.Application.Features.MessStaff.Validators
{
    public class AddStaffCommandValidator : AbstractValidator<AddStaffCommand>
    {
        public AddStaffCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required")
            .Must(e => !string.IsNullOrWhiteSpace(e) && !e.Trim().Any(char.IsWhiteSpace))
            .WithMessage("Email must not contain spaces in between")
            .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone must be 10 digits.");

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
        }
    }
}
