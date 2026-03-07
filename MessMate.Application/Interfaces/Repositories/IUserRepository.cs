using MessMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Interfaces.Repositories
{
    public  interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id); 
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task SaveChangesAsync();
        Task UpdateAsync(User user);
    }
}
