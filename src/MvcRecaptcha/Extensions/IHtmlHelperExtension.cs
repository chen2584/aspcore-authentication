using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcRecaptcha.Extensions
{
    public static class IHtmlHelperExtension
    {
        public static IHtmlContent Recaptcha(this IHtmlHelper htmlHelper, string siteKey, string callback)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<script src=\"https://www.google.com/recaptcha/api.js\"></script>");
            sb.AppendLine($"<div class=\"g-recaptcha\" data-sitekey=\"{siteKey}\" data-callback=\"{callback}\"></div>");

            var content = sb.ToString();
            return new HtmlString(content);
        }
    }
}