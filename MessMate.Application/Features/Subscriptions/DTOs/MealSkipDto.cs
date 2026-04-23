using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.DTOs
{
    public class MealSkipDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string MealSlot { get; set; } = null!;
        public DateTime MealDate { get; set; }
        public decimal RefundAmount { get; set; }
        public string RefundStatus { get; set; } = null!;
        public DateTime SkippedOn { get; set; }
    }
}
