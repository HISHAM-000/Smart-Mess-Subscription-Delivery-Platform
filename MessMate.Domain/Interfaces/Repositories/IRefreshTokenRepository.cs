using MessMate.Domain.Entities;

namespace MessMate.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByUserIdAsync(int userId);
        Task<RefreshToken?> GetByTokenAsync(string token);
    }
}