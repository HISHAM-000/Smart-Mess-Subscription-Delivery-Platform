using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.DTOs
{
    public class PendingOwnerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string MessName { get; set; } = null!;
        public string AuthorisedName { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}
