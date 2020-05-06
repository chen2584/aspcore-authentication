using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using MvcRecaptcha.Services;

namespace MvcRecaptcha.Attributes
{
    public class ValidateRecaptchaAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var capchaService = (CapchaService) serviceProvider.GetService(typeof(CapchaService));
            return new ValidateRecaptchaFilter(capchaService);
        }
    }
}