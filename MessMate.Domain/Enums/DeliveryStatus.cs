using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Enums
{
    public enum DeliveryStatus
    {
        Assigned = 1,
        OutForDelivery = 2,
        Delivered = 3,
        Failed = 4
    }
}
