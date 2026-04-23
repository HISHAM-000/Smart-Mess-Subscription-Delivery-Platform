using MessMate.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
    public class Mess:BaseEntity
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Rating { get; set; } = 0;
        public string AuthorisedName { get; set; } = null!;
        public string LicenseNumber { get; set; } = null!;
        public bool IsApproved { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public string? RejectionReason { get; set; }
        public bool IsRejected { get; set; } = false;


        public User Owner { get; set; } = null!;
        public ICollection<SubscriptionPlan> SubscriptionPlans { get; set; } = new List<SubscriptionPlan>();
    }
}
