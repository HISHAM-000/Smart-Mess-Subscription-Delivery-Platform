using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Application.Features.Auth.DTOs;
using MessMate.Application.Interfaces.Repositories;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Handlers
{
    public class LoginUserHandler:IRequestHandler<LoginUserCommand,LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public LoginUserHandler(IUserRepository userRepository,
                            ITokenService tokenService,
                            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoginResponse>Handle(LoginUserCommand request, 
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedException("Invalid credetials");
            var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password,user.PasswordHash);
            if (!passwordValid)
                throw new UnauthorizedException("invalid credentials");

            var accesToken = _tokenService.GenerateAccessToken(user);
            var refreshTokenValue = _tokenService.GenerateRefreshToken();

            var existingToken = await _refreshTokenRepository.GetByUserIdAsync(user.Id);
            if(existingToken != null)
            {
                existingToken.Token = refreshTokenValue;
                existingToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
                existingToken.RevokedAt = null;

                _refreshTokenRepository.Update(existingToken);
            }
            else
            {
                var refreshToken = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshTokenValue,
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                };
                await _refreshTokenRepository.AddAsync(refreshToken);
            }
            await _refreshTokenRepository.SaveChangesAsync();

            return new LoginResponse
            {
                AccessToken = accesToken,
                RefreshToken = refreshTokenValue
            };
        }
    }
}
