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
    public class MealSkipRepository : GenericRepository<MealSkip>,IMealSkipRepository
    {
        public MealSkipRepository(AppDbContext context) : base(context) { }

        public async Task<List<MealSkip>> GetByCustomerIdAsync(int customerId)
        {
            var result = await _context.MealSkips
                .Where(s => s.CustomerId == customerId && s.IsDeleted != true)
                .OrderByDescending(s => s.MealDate)
                .ToListAsync();

            return result;
        }
    }
}
