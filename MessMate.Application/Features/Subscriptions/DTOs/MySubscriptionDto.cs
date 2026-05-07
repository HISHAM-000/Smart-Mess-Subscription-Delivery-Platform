using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.DTOs
{
    public class MySubscriptionDto
    {
        public int Id { get; set; }
        public int MessId { get; set; }
        public string PlanName { get; set; } = null!;
        public string MessName { get; set; } = null!;
        public string PlanType { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public int SkippedMeals { get; set; }
        public bool IsPaused { get; set; }
        public DateOnly? PausedFrom { get; set; }
        public DateOnly? PausedUntil { get; set; }
        public int PausedDays { get; set; }
        public bool IsBreakfast { get; set; }
        public bool IsLunch { get; set; }
        public bool IsDinner { get; set; }
    }
}
