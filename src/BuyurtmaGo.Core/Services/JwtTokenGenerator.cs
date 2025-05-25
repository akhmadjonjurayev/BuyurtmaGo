using BuyurtmaGo.Core.Authentications.Entities;
using BuyurtmaGo.Core.Models.Options;
using Duende.IdentityModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuyurtmaGo.Core.Services
{
    public class JwtTokenGenerator
    {
        private readonly TokenGenerationOptions _tokenGenerationOptions;

        public JwtTokenGenerator(IOptions<TokenGenerationOptions> options)
        {
            this._tokenGenerationOptions = options.Value;
        }

        public string GenerateToken(User user, string roleName, IList<Claim>? claimsList = null)
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

        private int ToUnixTime(DateTime dateTime)
        {
            return (int)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
