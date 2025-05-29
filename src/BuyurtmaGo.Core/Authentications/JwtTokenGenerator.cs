using BuyurtmaGo.Core.Authentications.Entities;
using BuyurtmaGo.Core.Authentications.Options;
using BuyurtmaGo.Core.Interfaces;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuyurtmaGo.Core.Authentications
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly TokenGenerationOptions _tokenGenerationOptions;

        public JwtTokenGenerator(IOptions<TokenGenerationOptions> options)
        {
            _tokenGenerationOptions = options.Value;
        }

        public string GenerateToken(User user, string roleName, IList<Claim> claimsList = null)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtClaimTypes.Role, roleName),
                new Claim(JwtClaimTypes.IssuedAt, ToUnixTime(now).ToString(), ClaimValueTypes.Integer32)
            };

            if (claimsList is not null)
            {
                claims.AddRange(claimsList);
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenGenerationOptions.Secret));

            var jwt = new JwtSecurityToken(
            issuer: _tokenGenerationOptions.Issuer,
            audience: _tokenGenerationOptions.Audience,
            claims: claims,
            //notBefore: now,
            expires: now.Add(TimeSpan.FromMinutes(_tokenGenerationOptions.Expiration)),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = _tokenGenerationOptions.Audience,
                ValidIssuer = _tokenGenerationOptions.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenGenerationOptions.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private int ToUnixTime(DateTime dateTime)
        {
            return (int)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
