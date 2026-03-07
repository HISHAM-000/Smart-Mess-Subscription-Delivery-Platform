using MediatR;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Handlers
{
    public class LogoutHandler:IRequestHandler<LogoutCommand,bool>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<bool>Handle(LogoutCommand request,CancellationToken cancellationToken)
        {
            var token = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
            if (token == null)
                return false;

            token.RevokedAt = DateTime.UtcNow;

            _refreshTokenRepository.Update(token);
            await _refreshTokenRepository.SaveChangesAsync();
            return true;
        }
    }
}
