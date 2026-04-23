using FluentValidation;
using MessMate.Application.Features.MessStaff.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.MessStaff.Validators
{
    public class DeleteStaffCommandValidator : AbstractValidator<DeleteStaffCommand>
    {
        public DeleteStaffCommandValidator()
        {
            RuleFor(x => x.StaffId)
                .GreaterThan(0).WithMessage("Invalid staff id.");
        }
    }
}
