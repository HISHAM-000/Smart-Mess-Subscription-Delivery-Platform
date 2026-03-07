using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Application.Features.Auth.DTOs;
using MessMate.Application.Interfaces.Repositories;
using MessMate.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Handlers
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public RefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository, ITokenService tokenService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

            if (refreshToken == null || !refreshToken.IsActive)
                throw new UnauthorizedException("Invalid refresh token");

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId);

            if (user == null)
                throw new NotFoundException("User not found");
            var newAcessToken = _tokenService.GenerateAccessToken(user);

            return new LoginResponse
            {
                AccessToken = newAcessToken,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
