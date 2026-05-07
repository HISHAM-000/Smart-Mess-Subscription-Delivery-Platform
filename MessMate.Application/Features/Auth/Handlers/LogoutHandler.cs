//using MediatR;
//using MessMate.Application.Features.Auth.Commands;
//using MessMate.Application.Interfaces.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MessMate.Application.Features.Auth.Handlers
//{
//    public class LogoutHandler:IRequestHandler<LogoutCommand,bool>
//    {
//        private readonly IRefreshTokenRepository _refreshTokenRepository;

//        public LogoutHandler(IRefreshTokenRepository refreshTokenRepository)
//        {
//            _refreshTokenRepository = refreshTokenRepository;
//        }
//        public async Task<bool>Handle(LogoutCommand request,CancellationToken cancellationToken)
//        {
//            var token = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
//            if (token == null)
//                return false;

//            token.RevokedAt = DateTime.UtcNow;

//            _refreshTokenRepository.Update(token);
//            await _refreshTokenRepository.SaveChangesAsync();
//            return true;
//        }
//    }
//}
using MediatR;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Domain.Interfaces.Contracts;
using Microsoft.AspNetCore.Http;

public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutHandler(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            return false;

        var refreshToken = httpContext.Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return false;

        var token = await _unitOfWork.RefreshTokens.GetByTokenAsync(refreshToken);

        if (token == null)
            return false;

        token.RevokedAt = DateTime.UtcNow;

        // No SaveChanges in repo ❌
        await _unitOfWork.SaveChangesAsync(); // ✅ correct place

        return true;
    }
}