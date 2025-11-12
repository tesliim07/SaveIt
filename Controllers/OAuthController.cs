////using FoodSaver.Services.Interfaces;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Google;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace FoodSaver.Controllers
//{
//    [ApiController]
//    //[Route("api/OAuth")]
//    [Route("auth")]
//    public class OAuthController : ControllerBase
//    {
//        //private readonly ILogger _logger;
//        //private readonly IOAuthService _oAuthService;
//        //private readonly IUsersService _usersService;
//        //private readonly IJWTService _jwtService;
//        //public OAuthController(ILogger<OAuthController> logger, IOAuthService oAuthService, IUsersService usersService, IJWTService jwtService)
//        //{
//        //    _logger = logger;
//        //    _oAuthService = oAuthService;
//        //    _usersService = usersService;
//        //    _jwtService = jwtService;
//        //}


//        [HttpGet("login-google")]
//        [AllowAnonymous]
//        public IActionResult LoginWithGoogle()
//        {

//            //var redirectUrl = Url.Action(nameof(GoogleResponse), "OAuth");
//            //if (string.IsNullOrEmpty(redirectUrl))
//            //{
//            //    _logger.LogError("[OAuthController] Redirect Url is null");
//            //}
//            //var properties = _oAuthService.LoginViaGoogle(redirectUrl);
//            //return Challenge(properties, GoogleDefaults.AuthenticationScheme);

//            var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth");
//            var props = new AuthenticationProperties { RedirectUri = redirectUrl };
//            return Challenge(props, GoogleDefaults.AuthenticationScheme);
//        }



//        [HttpGet("google-callback")]
//        [AllowAnonymous]
//        public IActionResult GoogleCallback()
//        {

//            if (!(User?.Identity?.IsAuthenticated ?? false))
//            {
//                return BadRequest("Not authenticated from Google.");
//            }

//            var email = User.FindFirstValue(ClaimTypes.Email);
//            var name = User.FindFirstValue(ClaimTypes.Name);
//            //var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            return Ok(new
//            {
//                message = "Google login success (minimal)",
//                email,
//                name,
//                //providerId
//            });


//            //var result = await HttpContext.AuthenticateAsync("External");
//            //if (!result.Succeeded || result.Principal == null)
//            //{
//            //    _logger.LogError("Google authentication failed or external principal missing.");
//            //    return BadRequest("Google authentication failed.");
//            //}
//            ////Allows me to ensure i am getting Google Account Identity and nothing else
//            //var googleIdentity = result.Principal?.Identities.FirstOrDefault(i => i.AuthenticationType == GoogleDefaults.AuthenticationScheme).Claims;
//            //if (googleIdentity == null)
//            //{
//            //    _logger.LogError("[OAuthController] Identity not provided by Google");
//            //    return StatusCode(403);
//            //}
//            //;
//            //var email = googleIdentity.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
//            //var name = googleIdentity.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
//            //var providerId = googleIdentity.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

//            //_usersService.CreateUserFromGoogleResponse(email, name, providerId);
//            //var jwt = _jwtService.GenerateToken(email, name);

//            //// Optional: clear external cookie after use
//            //await HttpContext.SignOutAsync("External");

//            //return Ok(new
//            //{
//            //    Message = "Login successful",
//            //    Email = email,
//            //    Name = name,
//            //    ProviderId = providerId,
//            //    Token = jwt
//            //});

//        }

//        //[HttpGet("secure")]
//        //[Authorize]  // requires a valid JWT
//        //public IActionResult Secure()
//        //{
//        //    return Ok(new { message = "You reached a protected endpoint!" });
//        //}



//    }
//}



//This works
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Google;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;

//[ApiController]
//[Route("auth")]
//public class AuthController : ControllerBase
//{
//    [HttpGet("login-google")]
//    [AllowAnonymous]
//    public IActionResult LoginWithGoogle()
//    {
//        var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth");
//        var props = new AuthenticationProperties { RedirectUri = redirectUrl };
//        return Challenge(props, GoogleDefaults.AuthenticationScheme);
//    }

//    [HttpGet("google-callback")]
//    [AllowAnonymous]
//    public IActionResult GoogleCallback()
//    {
//        if (!(User?.Identity?.IsAuthenticated ?? false))
//        {
//            return BadRequest("Not authenticated from Google.");
//        }

//        var email = User.FindFirstValue(ClaimTypes.Email);
//        var name = User.FindFirstValue(ClaimTypes.Name);

//        return Ok(new
//        {
//            message = "Google login success (minimal)",
//            email,
//            name
//        });
//    }
//}



//Testing

using FoodSaver.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodSaver.Controllers
{
    [ApiController]
    [Route("auth")]
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
        [AllowAnonymous]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "OAuth");
            var props = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }



        [HttpGet("google-callback")]
        [AllowAnonymous]
        public IActionResult GoogleCallback()
        {

            if (!(User?.Identity?.IsAuthenticated ?? false))
            {
                return BadRequest("Not authenticated from Google.");
            }

            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name);
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _usersService.CreateUserFromGoogleResponse(email, name, providerId);
            var jwt = _jwtService.GenerateToken(email, name);

            return Ok(new
            {
                message = "Google login success (minimal)",
                email,
                name,
                providerId,
                Token = jwt
            });

        }

        //[HttpGet("secure")]
        //[Authorize]  // requires a valid JWT
        //public IActionResult Secure()
        //{
        //    return Ok(new { message = "You reached a protected endpoint!" });
        //}



    }
}