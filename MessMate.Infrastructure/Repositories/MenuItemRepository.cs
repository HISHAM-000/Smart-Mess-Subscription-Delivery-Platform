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
    public class MenuItemRepository : GenericRepository<MenuItem>,IMenuItemRepository
    {
        public MenuItemRepository(AppDbContext context) : base(context) { }

        public async Task<List<MenuItem>> GetByMenuIdAsync(int menuId)
        {
            return await _context.MenuItems
                .Where(x => x.MenuId == menuId && x.IsDeleted != true)
                .ToListAsync();
        }

    }
}
