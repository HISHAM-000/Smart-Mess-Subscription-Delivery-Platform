using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Orders.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string MessName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string MealSlot { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
        public string DishName { get; set; } = null!; 
    }
}
