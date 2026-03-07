using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Interfaces.Repositories
{
    public interface IRoleApplicationRepository
    {
        Task<RoleApplication> AddAsync(RoleApplication application);

        Task<RoleApplication?> GetByIdAsync(Guid id);
        Task<RoleApplication?> GetUserPendingApplication(Guid userId, UserRole role);
        Task<List<RoleApplication>> GetPendingApplicationsAsync();

        Task UpdateAsync(RoleApplication application);
    }
}
