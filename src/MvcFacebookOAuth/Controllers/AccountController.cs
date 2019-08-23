using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApiFacebookOAuth.Controllers
{
    public class AccountController : ControllerBase
    {
        readonly IAuthenticationSchemeProvider authenticationSchemaProvider;
        public AccountController(IAuthenticationSchemeProvider authenticationSchemaProvider)
        {
            this.authenticationSchemaProvider = authenticationSchemaProvider;
        }

        // // GET api/values
        // public async Task<ActionResult> Login()
        // {
        //     var displayName = (await authenticationSchemaProvider.GetAllSchemesAsync()).Select(x => x.DisplayName);
        //     System.Console.WriteLine("Hello World!");
        //     System.Console.WriteLine(string.Join(", ", displayName));
        //     return Ok();
        // }

        public ActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Facebook");
        }

        public async Task<ActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
