using MessMate.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User
                    ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return userId != null ? int.Parse(userId) : 0;
            }
        }
        public string Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User
                    ?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            }
        }
        public bool IsAuthenticated =>
    _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
