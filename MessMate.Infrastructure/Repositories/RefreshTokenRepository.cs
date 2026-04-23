
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
    public class RefreshTokenRepository : GenericRepository<RefreshToken>,IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context) { }

        public async Task<RefreshToken?> GetByUserIdAsync(int userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);
        }

    }
}
