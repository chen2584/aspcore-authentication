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
        readonly HttpContext _context;
        readonly IHttpClientFactory _httpClientFactory;
        readonly ILogger<CapchaService> _logger;
        public CapchaService(AppSetting setting,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            ILogger<CapchaService> logger)
        {
            _setting = setting;
            _context = httpContextAccessor.HttpContext;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<bool> IsPassedAsync()
        {
            if (_context.Request.Form.TryGetValue("g-recaptcha-response", out var token))
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_setting.GoogleReCaptcha.SecretKey}&response={token}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var data = JsonSerializer.Deserialize<RecaptchaVerifyResponse>(jsonString);
                    if (data.Success)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}