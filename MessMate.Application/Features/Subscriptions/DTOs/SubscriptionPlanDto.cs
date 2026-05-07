using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.DTOs
{
    public class SubscriptionPlanDto
    {
        public int Id { get; set; }
        public int MessId { get; set; }
        public string Name { get; set; } = null!;
        public string PlanType { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int MinActiveDays { get; set; }
        public bool IsBreakfast { get; set; }
        public bool IsLunch { get; set; }
        public bool IsDinner { get; set; }
    }
}
