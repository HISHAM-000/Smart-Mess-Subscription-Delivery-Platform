using MediatR;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Application.Features.Auth.DTOs;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MessMate.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public RefreshTokenService(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> RefreshTokenAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
                throw new UnauthorizedAccessException("No HttpContext");

            var refreshToken = httpContext.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedAccessException("No refresh token");

            var storedToken = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (storedToken == null || !storedToken.IsActive)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var user = storedToken.User;

            var newAccessToken = _tokenService.GenerateAccessToken(user);

            storedToken.RevokedAt = DateTime.UtcNow;

            var newRefreshToken = new RefreshToken
            {
                Token = _tokenService.GenerateRefreshToken(),
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}
