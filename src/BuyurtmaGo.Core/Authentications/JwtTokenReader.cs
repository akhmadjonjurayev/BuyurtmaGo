using BuyurtmaGo.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuyurtmaGo.Core.Authentications
{
    public class JwtTokenReader
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenReader(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetUserId()
        {
            var value = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(value)) throw new Exception(ErrorCodes.TokenNotFound.ToString());

            if (!long.TryParse(value, out var userId))
            {
                throw new InvalidCastException($"Cannot convert '{value}' to long for user ID.");
            }

            return userId;
        }

        public string? GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }

        public string? GetUserRole()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
        }

        public string? GetClaimValue(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value;
        }
    }
}
