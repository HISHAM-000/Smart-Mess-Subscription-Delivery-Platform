using MessMate.Domain.Common;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
    public class Delivery : BaseEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int StaffId { get; set; }        
        public int AssignedBy { get; set; }   
        public DeliveryStatus Status { get; set; } = DeliveryStatus.Assigned;
        public string OTP { get; set; } = null!;
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveredAt { get; set; }
        public string DeliveryAddress { get; set; } = null!;

        public Order Order { get; set; } = null!;
        public User Staff { get; set; } = null!;
    }
}
