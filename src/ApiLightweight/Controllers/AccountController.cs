using System.Security.Claims;
using System.Threading.Tasks;
using ApiLightweight.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ApiLightweight.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginInput input)
        {
            if (input.Username.Equals("admin") && input.Password.Equals("admin"))
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, "chen2584"));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, "Chen"));
                identity.AddClaim(new Claim(ClaimTypes.Surname, "Semapat"));

                identity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);

                return Ok();
            }

            return Unauthorized();
        }

        [HttpDelete]
        public ActionResult LogoutAsync()
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     HttpContext.SignOutAsync();
            // }
            HttpContext.SignOutAsync();

            return Conflict();
        }
    }
}