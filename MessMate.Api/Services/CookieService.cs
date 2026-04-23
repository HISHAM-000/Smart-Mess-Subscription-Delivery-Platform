using MessMate.Application.Common.Models;
using Microsoft.Extensions.Options;

namespace MessMate.Api.Services
{
    public class CookieService
    {
        private readonly JwtSettings _jwtSettings;
        public CookieService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }
        public void SetAuthCookies(HttpResponse response, string accessToken, string refreshToken)
        {
            var accessOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes)
            };

            var refreshOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            response.Cookies.Append("accessToken", accessToken, accessOptions);
            response.Cookies.Append("refreshToken", refreshToken, refreshOptions);
        }

        public void ClearAuthCookies(HttpResponse response)
        {
            response.Cookies.Delete("accessToken");
            response.Cookies.Delete("refreshToken");
        }
    }
}
