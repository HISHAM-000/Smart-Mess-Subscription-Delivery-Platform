using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Delivery.DTOs
{
    public class DeliveryHistoryDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string MessName { get; set; } = null!;
        public string MealSlot { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime AssignedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
