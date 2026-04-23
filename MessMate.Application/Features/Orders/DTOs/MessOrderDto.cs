using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Orders.DTOs
{
    public class MessOrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public string DeliveryAddress { get; set; } = null!;
        public string MealSlot { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
