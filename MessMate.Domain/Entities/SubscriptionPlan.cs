using MessMate.Domain.Common;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
    public class SubscriptionPlan:BaseEntity
    {
        public int Id { get; set; }
        public int MessId { get; set; }
        public string Name { get; set; } = null!;
        public PlanType PlanType { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int MinActiveDays { get; set; }
        public bool IsBreakfast { get; set; }
        public bool IsLunch { get; set; }
        public bool IsDinner { get; set; }
        public bool IsActive { get; set; } = true;

        public Mess Mess { get; set; } = null!;
        public ICollection<CustomerSubscription> Subscriptions { get; set; } = new List<CustomerSubscription>();
    }
}
