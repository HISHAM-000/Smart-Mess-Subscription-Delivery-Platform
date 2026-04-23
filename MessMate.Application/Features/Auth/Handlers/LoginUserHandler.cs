using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Application.Features.Auth.DTOs;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Interfaces.Contracts;
using MessMate.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUser;
        public LoginUserHandler(IUnitOfWork unitOfWork, ITokenService tokenService,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _currentUser = currentUser;
        }

        public async Task<LoginResponse> Handle(LoginUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email.Trim());
            if (user == null)
                throw new UnauthorizedException("Invalid credetials");

            var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password.Trim(), user.PasswordHash);
            if (!passwordValid)
                throw new UnauthorizedException("invalid credentials");

            if (user.IsDeleted == true && user.IsRejected)
                throw new UnauthorizedException(
                    $"Your account has been rejected. Reason: {user.RejectionReason}");

            if (user.IsDeleted == true)
                throw new UnauthorizedException(
                    "Your account no longer exists.");

            if (!user.IsActive)
                throw new UnauthorizedException(
                    "Your account has been deactivated. Please contact support.");

            var accesToken = _tokenService.GenerateAccessToken(user);
            var refreshTokenValue = _tokenService.GenerateRefreshToken();

            var existingToken = await _unitOfWork.RefreshTokens.GetByUserIdAsync(user.Id);
            if (existingToken != null)
            {
                existingToken.Token = refreshTokenValue;
                existingToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
                existingToken.RevokedAt = null;

                await _unitOfWork.RefreshTokens.UpdateAsync(existingToken);
            }
            else
            {
                var refreshToken = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshTokenValue,
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                };
                await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            }
            await _unitOfWork.SaveChangesAsync();

            return new LoginResponse
            {
                AccessToken = accesToken,
                RefreshToken = refreshTokenValue,
                Role = user.Role.ToString()
            };
        }
    }
}
