using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
    }
}
