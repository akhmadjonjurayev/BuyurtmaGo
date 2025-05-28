using BuyurtmaGo.Core.Authentications.Entities;
using BuyurtmaGo.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BuyurtmaGo.Core.Managers
{
    public record SignInViewModel(string UserName, string Password);

    public class AuthManager(UserManager<User> _userManager,
        IJwtTokenGenerator _jwtTokenManager)
    {
        public async ValueTask<string> SignAsync(SignInViewModel signInView)
        {
            var user = await _userManager.FindByNameAsync(signInView.UserName);

            if (user is null) throw new Exception($"User not found, userName = {signInView.UserName}");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, signInView.Password);

            if (!isPasswordValid) throw new Exception($"UserName or Password is not Correct");

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenManager.GenerateToken(user, roles.FirstOrDefault());

            return token;
        }
    }
}
