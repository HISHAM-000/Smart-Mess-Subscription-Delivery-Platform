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
    public class MenuRepository : GenericRepository<Menu>,IMenuRepository
    {
        public MenuRepository(AppDbContext context) : base(context) { }

        public async Task<Menu?> GetByMessAndDayAsync(int messId, DayOfWeek day)
        {
            return await _context.Menus
                .FirstOrDefaultAsync(m =>
                    m.MessId == messId &&
                    m.Day == day &&
                    m.IsDeleted != true);
        }
    }
}
