using BuyurtmaGo.Core.Authentications.Models;
using BuyurtmaGo.Core.Extentions;
using BuyurtmaGo.Core.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuyurtmaGo.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly AuthManager _authManager;

        public AuthController(AuthManager authManager)
        {
            this._authManager = authManager;
        }

        [HttpPost("signin")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> SignIn([FromBody] SignInViewModel viewModel)
        {
            return Result(await _authManager.SignAsync(viewModel));
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserInfoModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> GetUserInfo()
        {
            return Result(await _authManager.GetUserInfo());
        }
    }
}
