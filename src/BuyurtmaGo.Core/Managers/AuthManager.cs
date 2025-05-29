using BuyurtmaGo.Core.Authentications;
using BuyurtmaGo.Core.Authentications.Entities;
using BuyurtmaGo.Core.Authentications.Models;
using BuyurtmaGo.Core.Authentications.Options;
using BuyurtmaGo.Core.Enums;
using BuyurtmaGo.Core.Interfaces;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BuyurtmaGo.Core.Managers
{
    public record SignInViewModel(string UserName, string Password);

    public class AuthManager(UserManager<User> _userManager,
        BuyurtmaGoDb _db,
        JwtTokenReader _jwtTokenReader,
        IJwtTokenGenerator _jwtTokenManager,
        IOptions<TokenGenerationOptions> _options,
        IHttpContextAccessor _httpContextAccessor)
    {
        private const string DefaultTokenProvider = nameof(DefaultTokenProvider);
        public async ValueTask<OperationResult<TokenResponse, ErrorCodes>> SignAsync(SignInViewModel signInView)
        {
            var user = await _userManager.FindByNameAsync(signInView.UserName);

            if (user is null) return ErrorCodes.UserNotFound;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, signInView.Password);

            if (!isPasswordValid) return ErrorCodes.PasswordIsWrong;

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenManager.GenerateToken(user, roles.FirstOrDefault());

            return new TokenResponse(token);
        }

        public bool IsRefreshTokenValid(ClaimsPrincipal principal)
        {
            var expClaim = principal.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.Expiration);
            if (expClaim == null)
            {
                return false;
            }

            if (!long.TryParse(expClaim.Value, out var tokenExpireTime))
            {
                return false;
            }

            //Because token's expiration already added in token claim, we subtract them
            var expiresInDays = _options.Value.RefreshTokenExpireInDays - _options.Value.Expiration;

            var expirationDate = DateTimeOffset.FromUnixTimeSeconds(tokenExpireTime).AddDays(expiresInDays);

            return expirationDate > DateTimeOffset.Now;
        }

        public async ValueTask<OperationResult<UserInfoModel, ErrorCodes>> GetUserInfo()
        {
            var userId = _jwtTokenReader.GetUserId();

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            if (user is null) return ErrorCodes.UserNotFound;

            var roles = await _userManager.GetRolesAsync(user);

            var userInfo = new UserInfoModel(
                user.Id,
                user.UserName,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                roles.FirstOrDefault(),
                user.AvatarId
            );

            return userInfo;
        }

        public async Task<string> GetRefreshToken(User user) =>
            await _userManager.GetAuthenticationTokenAsync(user, DefaultTokenProvider, "RefreshToken");

        private async ValueTask<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            await _userManager.SetAuthenticationTokenAsync(
                user,
                DefaultTokenProvider,
                "RefreshToken",
                refreshToken
            );
            return refreshToken;
        }
    }
}
