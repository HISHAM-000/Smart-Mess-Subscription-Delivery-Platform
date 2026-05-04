
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Repositories;
using MessMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace MessMate.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>,IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        }
        public async Task<User?> GetByLicenseNumberAsync(string number)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.LicenseNumber == number);
        }
        public async Task<List<User>> GetPendingOwnersAsync()
        {
            return await _context.Users
        .Where(u => u.Role == UserRole.MessOwner
                 && !u.IsActive
                 && u.IsDeleted != true)
        .OrderByDescending(u => u.CreatedOn)
        .ToListAsync();
        }

        public async Task<List<User>> GetStaffByMessIdAsync(int messId)
        {
            return await _context.Users
                .Where(u =>
                    u.Role == UserRole.MessStaff &&
                    u.MessId == messId &&
                    u.IsDeleted != true)
                .OrderByDescending(u => u.CreatedOn)
                .ToListAsync();
        }

        
    }
}
