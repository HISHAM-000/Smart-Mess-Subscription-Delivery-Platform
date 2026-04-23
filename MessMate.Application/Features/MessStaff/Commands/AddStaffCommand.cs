using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.MessStaff.Commands
{
    public record AddStaffCommand(
        string Name,
        string Email,
        string PhoneNumber,
        string Password
    ) : IRequest<int>;
}
