using MessMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Interfaces.Repositories
{
    public interface IDeliveryRepository : IGenericRepository<Delivery>
    {
        Task<Delivery?> GetByOrderIdAsync(int orderId);
        Task<List<Delivery>> GetByStaffIdAsync(int staffId);
        Task<bool> ExistsForOrderAsync(int orderId);
    }
}
