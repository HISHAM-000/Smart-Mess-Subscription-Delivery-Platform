using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.DTOs
{
    public class PendingMessDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string MessName { get; set; } = null!;
        public string AuthorisedName { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string OwnerName { get; set; } = null!;
        public string OwnerEmail { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }
}
