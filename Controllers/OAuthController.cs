//Testing

using FoodSaver.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodSaver.Controllers
{
    [ApiController]
    [Route("OAuth")]
    public class OAuthController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUsersService _usersService;
        private readonly IJWTService _jwtService;
        public OAuthController(ILogger<OAuthController> logger, IUsersService usersService, IJWTService jwtService)
        {
            _logger = logger;
            _usersService = usersService;
            _jwtService = jwtService;
        }


        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "OAuth");
            var props = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }



        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {

            // Ask ASP.NET Core to authenticate using the "External" cookie scheme
            var result = await HttpContext.AuthenticateAsync("External");

            if (!result.Succeeded || result.Principal == null)
            {
                return BadRequest("Not authenticated from Google.");
            }

            var externalUser = result.Principal;

            var email = externalUser.FindFirstValue(ClaimTypes.Email);
            var name = externalUser.FindFirstValue(ClaimTypes.Name);
            var providerId = externalUser.FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = await _usersService.CreateUserFromGoogleResponse(email, name, providerId);
            var jwt = _jwtService.GenerateToken(providerId, email);
            //var redirectUrl = $"http://localhost:5173/oauth-callback?token={jwt}";
            var redirectUrl = $"https://saveit-frontend-klwdw.ondigitalocean.app/oauth-callback?token={jwt}";
            return Redirect(redirectUrl);
        }
    }
}