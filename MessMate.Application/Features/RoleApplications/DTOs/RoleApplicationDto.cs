using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.RoleApplications.DTOs
{
    public class RoleApplicationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string RequestedRole { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
