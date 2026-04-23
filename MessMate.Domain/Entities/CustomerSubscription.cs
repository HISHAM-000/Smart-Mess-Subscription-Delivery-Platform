using MessMate.Domain.Common;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
    public class CustomerSubscription : BaseEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PlanId { get; set; }
        public int MessId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public string DeliveryAddress { get; set; } = null!;
        public double DeliveryLat { get; set; }
        public double DeliveryLng { get; set; }
        public int SkippedMeals { get; set; } = 0;
        public int PausedDays { get; set; } = 0;
        public DateTime? PausedFrom { get; set; }
        public DateTime? PausedUntil { get; set; }

        public User Customer { get; set; } = null!;
        public SubscriptionPlan Plan { get; set; } = null!;
        public Mess Mess { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<MealSkip> MealSkips { get; set; } = new List<MealSkip>();
    }
}
