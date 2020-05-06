using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using MvcRecaptcha.Services;

namespace MvcRecaptcha.Attributes
{
    public class ValidateRecaptchaFilter : IAsyncActionFilter
    {
        readonly CapchaService _capchaService;
        public ValidateRecaptchaFilter(CapchaService capchaService)
        {
            _capchaService = capchaService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var recaptcha = await _capchaService.ValidateAsync(context.HttpContext);
            if (recaptcha == null || !recaptcha.Success)
                context.ModelState.AddModelError("Recaptcha", "There was an error validating the google recaptcha response. Please try again, or contact the site owner.");

            await next();
        }
    }
}