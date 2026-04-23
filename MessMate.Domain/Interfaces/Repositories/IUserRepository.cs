using MessMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Interfaces.Repositories
{
    public interface IUserRepository:IGenericRepository<User> 
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByLicenseNumberAsync(string number);
        Task<List<User>> GetPendingOwnersAsync();
        Task<List<User>> GetStaffByMessIdAsync(int messId);
    }
}
