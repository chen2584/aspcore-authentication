using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MvcRecaptcha.Models;
using MvcRecaptcha.Models.Services;

namespace MvcRecaptcha.Services
{
    public class CapchaService
    {
        readonly AppSetting _setting;
        public CapchaService(AppSetting setting)
        {
            _setting = setting;
        }

        public async Task<RecaptchaVerifyResponse> ValidateAsync(HttpContext context)
        {
            if (context.Request.Form.TryGetValue("g-recaptcha-response", out var token))
            {
                var client = new HttpClient();
                var response = await client.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_setting.GoogleReCaptcha.SecretKey}&response={token}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<RecaptchaVerifyResponse>(jsonString);
                    return data;
                }
            }
            return null;
        }
    }
}