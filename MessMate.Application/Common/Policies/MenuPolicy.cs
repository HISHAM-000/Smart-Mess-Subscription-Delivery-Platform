using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Common.Policies
{
    public static class MenuPolicy
    {
        public static async Task<bool> IsMenuLockedAsync(
            IUnitOfWork unitOfWork,
            int messId,
            DayOfWeek menuDay)
        {
            var orders = await unitOfWork.Orders
                .GetByMessIdAndDateAsync(messId, DateTime.Now.Date);

            return orders.Any(o =>
                o.OrderDate.DayOfWeek == menuDay &&
                o.IsDeleted != true);
        }
    }
}
