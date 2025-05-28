using BuyurtmaGo.Core.Authentications.Entities;
using System.Security.Claims;

namespace BuyurtmaGo.Core.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, string roleName, IList<Claim>? claimsList = null);
    }
}
