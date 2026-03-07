using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.RoleApplications.Commands
{
    public class ApproveApplicationCommand:IRequest<Unit>
    {
        public Guid ApplicationId { get; set; }
    }
}
