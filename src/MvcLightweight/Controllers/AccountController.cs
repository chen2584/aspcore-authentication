using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MvcLightweight.Controllers
{
    public class AccountController : Controller
    {
        public async Task<IActionResult> Login()
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, "chen2584"));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, "Chen"));
            identity.AddClaim(new Claim(ClaimTypes.Surname, "Semapat"));

            identity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));

            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddMinutes(10)
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}