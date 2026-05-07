    using MessMate.Application.Features.Auth.DTOs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace MessMate.Application.Interfaces.Services
    {
        public interface IRefreshTokenService
        {
            Task<LoginResponse> RefreshTokenAsync();
        }
    }
