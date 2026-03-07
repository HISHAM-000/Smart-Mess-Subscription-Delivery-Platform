using MessMate.Domain.Common;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
    public class RoleApplication:BaseEntity
    {
        public Guid UserId { get; set; }

        public UserRole RequestedRole { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        public User User { get; set; } = null!;
    }
}
