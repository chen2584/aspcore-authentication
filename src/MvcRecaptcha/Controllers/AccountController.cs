using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcRecaptcha.Models;
using MvcRecaptcha.Models.Controllers.Account;
using MvcRecaptcha.Services;

namespace MvcRecaptcha.Controllers
{
    public class AccountController : Controller
    {
        readonly CapchaService _capchaService;
        public AccountController(CapchaService capchaService)
        {
            _capchaService = capchaService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInput input)
        {

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var isCapchaPassed = await _capchaService.IsPassedAsync();
            if (!isCapchaPassed)
            {
                return Ok("Capcha fail");
            }

            if (input.Username.Equals("admin") && input.Password.Equals("admin"))
            {
                return Ok("Login Success");
            }
            ModelState.AddModelError("", "Username or Password doesn't exist");
            return View(input);
        }
    }
}
