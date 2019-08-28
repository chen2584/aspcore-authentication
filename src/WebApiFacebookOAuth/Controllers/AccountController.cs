using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        [HttpGet("facebook/signin")]
        public async Task<ActionResult<string>> SignIn([Required] string code)
        {
            var accessToken = await _facebookOAuthService.GetAccessTokenAsync(code);
            var userInfo = await _facebookOAuthService.GetUserInfoAsync(accessToken);

            await _authenticationService.AddFacebookUserIfNotExistAsync(userInfo);
            return Ok(userInfo);
        }
    }
}
