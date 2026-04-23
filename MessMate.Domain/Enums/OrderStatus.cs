using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        Preparing = 2,
        OutForDelivery = 3,
        Delivered = 4,
        Skipped = 5,
        Cancelled = 6
    }
}
