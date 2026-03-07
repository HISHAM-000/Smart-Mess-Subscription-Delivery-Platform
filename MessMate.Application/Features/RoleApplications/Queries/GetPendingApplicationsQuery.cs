using MediatR;
using MessMate.Application.Features.RoleApplications.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.RoleApplications.Queries
{
    public class GetPendingApplicationsQuery:IRequest<List<RoleApplicationDto>>
    {
    }
}
