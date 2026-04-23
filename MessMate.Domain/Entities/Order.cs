using MessMate.Domain.Common;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int CustomerId { get; set; }
        public int MessId { get; set; }
        public DateTime OrderDate { get; set; }
        public MealSlot MealSlot { get  ; set; }   
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal Amount { get; set; }
        public int? MenuItemId { get; set; }

        public MenuItem? MenuItem { get; set; }
        public CustomerSubscription Subscription { get; set; } = null!;
        public User Customer { get; set; } = null!;
        public Mess Mess { get; set; } = null!;
        public Delivery? Delivery { get; set; }
        public MealSkip? MealSkip { get; set; }
    }
}
