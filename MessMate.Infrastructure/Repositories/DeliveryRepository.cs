using MessMate.Domain.Entities;
using MessMate.Domain.Interfaces.Repositories;
using MessMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Infrastructure.Repositories
{
    public class DeliveryRepository : GenericRepository<Delivery>,IDeliveryRepository
    {
        public DeliveryRepository(AppDbContext context) : base(context) {}

        public async Task<Delivery?> GetByOrderIdAsync(int orderId)
        {
            var order = await _context.Deliveries
                .Include(d => d.Staff)
                .Include(d => d.Order)
                .FirstOrDefaultAsync(d =>
                    d.OrderId == orderId &&
                    d.IsDeleted != true);

            return order;
        }

        public async Task<List<Delivery>> GetByStaffIdAsync(int staffId)
        {
            var staffs = await _context.Deliveries
                .Include(d => d.Order)
                    .ThenInclude(o => o.Mess)
                .Where(d =>
                    d.StaffId == staffId &&
                    d.IsDeleted != true)
                .OrderByDescending(d => d.AssignedAt)
                .ToListAsync();

            return staffs;
        }

        public async Task<bool> ExistsForOrderAsync(int orderId)
        {
            return await _context.Deliveries
                .AnyAsync(d =>
                    d.OrderId == orderId &&
                    d.IsDeleted != true);
        }
           

    }
}
