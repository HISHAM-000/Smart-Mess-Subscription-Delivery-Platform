using MessMate.Application.Interfaces.Repositories;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Infrastructure.Repositories
{
    public class RoleApplicationRepository:IRoleApplicationRepository
    {
        private readonly AppDbContext _context;
        public RoleApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RoleApplication> AddAsync(RoleApplication application)
        {
            _context.RoleApplications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<RoleApplication?> GetByIdAsync(Guid id)
        {
            return await _context.RoleApplications.FindAsync(id);
        }

        public async Task<List<RoleApplication>> GetPendingApplicationsAsync()
        {
            return await _context.RoleApplications
                .Where(x => x.Status == ApplicationStatus.Pending)
                .ToListAsync();
        }

        public async Task<RoleApplication?> GetUserPendingApplication(Guid userId, UserRole role)
        {
            return await _context.RoleApplications
                .FirstOrDefaultAsync(a => a.UserId == userId && a.RequestedRole == role
                && a.Status == ApplicationStatus.Pending);
        }

        public async Task UpdateAsync(RoleApplication application)
        {
            _context.RoleApplications.Update(application);
            await _context.SaveChangesAsync();
        }
    }
}
