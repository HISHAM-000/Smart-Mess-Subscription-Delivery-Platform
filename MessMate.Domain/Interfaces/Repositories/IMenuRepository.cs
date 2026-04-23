using MessMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Interfaces.Repositories
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<Menu?> GetByMessAndDayAsync(int messId, DayOfWeek day);
    }
}
