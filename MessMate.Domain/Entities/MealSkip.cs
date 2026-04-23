using MessMate.Domain.Common;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
     public class MealSkip : BaseEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int SubscriptionId { get; set; }
        public int CustomerId { get; set; }
        public DateTime MealDate { get; set; }
        public string MealSlot { get; set; } = null!;
        public decimal RefundAmount { get; set; }
        public RefundStatus RefundStatus { get; set; } = RefundStatus.Pending;

        public Order Order { get; set; } = null!;
        public CustomerSubscription Subscription { get; set; } = null!;
        public User Customer { get; set; } = null!;
    }
}
