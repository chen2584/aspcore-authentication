using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApiFacebookOAuth.Models;
using WebApiFacebookOAuth.Services;

namespace WebApiFacebookOAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly AuthenticationService _authenticationService;
        readonly FacebookOAuthService _facebookOAuthService;
        public AccountController(FacebookOAuthService facebookOAuthService, AuthenticationService authenticationService)
        {
            _facebookOAuthService = facebookOAuthService;
            _authenticationService = authenticationService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var queryStrings = Request.Query;
            foreach (var queryString in queryStrings)
            {
                Console.WriteLine($"{queryString.Key}: {queryString.Value}");
            }
            // Console.WriteLine(JsonConvert.SerializeObject(value));
            return Ok();
        }

        [HttpGet("facebook/oauthuri")]
        public ActionResult<string> GetFacebookAuthorizationUri()
        {
            return _facebookOAuthService.GetAuthorizationUrl();
        }

        private SignInOutput CreateSignInOutput(string accessToken, DateTime expires)
        {
            var output = new SignInOutput
            {
                AccessToken = accessToken,
                ExpiresDate = expires
            };
            return output;
        }

        [HttpGet("facebook/signin")]
        public async Task<ActionResult<SignInOutput>> SignIn([Required] string code)
        {
            var oAuthAccessToken = await _facebookOAuthService.GetAccessTokenAsync(code);
            var userInfo = await _facebookOAuthService.GetUserInfoAsync(oAuthAccessToken);

            var userProfile = await _authenticationService.GetUserProfileAsync(userInfo);
            var (accessToken, expires) = _authenticationService.GenerateAuthorizationToken(userProfile);
            return CreateSignInOutput(accessToken, expires);
        }
    }
}
