
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
    public class MessRepository : GenericRepository<Mess>,IMessRepository
    {
        public MessRepository(AppDbContext context) : base(context) { }

        public async Task<Mess> GetByOwnerIdAsync(int id)
        {
            var mess = await _context.Messes.FirstOrDefaultAsync(x => x.OwnerId == id);
            return mess;
        }
        public async Task<bool> ExistsByOwnerIdAsync(int ownerId)
        {
            return await _context.Messes.AnyAsync(m => m.OwnerId == ownerId && m.IsDeleted != true);
        }

        public async Task<List<Mess>> GetAllMessesAsync(bool approvedOnly)
        {
            var query = _context.Messes
                .Where(m => m.IsDeleted != true);

            if (approvedOnly)
                query = query.Where(m => m.IsApproved && m.IsActive);

            return await query
                .OrderByDescending(m => m.CreatedOn)
                .ToListAsync();
        }

        public async Task<List<Mess>> GetPendingMessesAsync()
        {
            return await _context.Messes
            .Include(m => m.Owner)
            .Where(m => !m.IsApproved
                     && m.IsDeleted != true)
            .OrderByDescending(m => m.CreatedOn)
            .ToListAsync();
        }
   
    }
}
