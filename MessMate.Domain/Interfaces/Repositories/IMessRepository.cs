using MessMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Interfaces.Repositories
{
    public interface IMessRepository:IGenericRepository<Mess>
    {
        Task<Mess> GetByOwnerIdAsync(int id);
        Task<bool> ExistsByOwnerIdAsync(int ownerId);
        Task<List<Mess>> GetAllMessesAsync(bool approvedOnly);
        Task<List<Mess>> GetPendingMessesAsync();
    }
}
