using MediatR;
using MessMate.Application.Features.MessStaff.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.MessStaff.Queries
{
    public record GetMyStaffQuery(int? MessId) : IRequest<List<StaffDto>>;
}
