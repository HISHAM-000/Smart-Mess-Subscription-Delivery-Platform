using FluentValidation;
using MessMate.Application.Features.Mess.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Validators
{
    public class CreateMessValidator:AbstractValidator<CreateMessCommand>
    {
        public CreateMessValidator()
        {
            RuleFor(x => x.PostalCode)
            .NotEmpty()
            .Matches(@"^\d{6}$").WithMessage("Postal code must be 6 digits.");
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90); 
            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);
        }
    }
}
