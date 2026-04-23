using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.MessStaff.Commands
{
    public record DeleteStaffCommand(int StaffId) : IRequest<bool>;
}
