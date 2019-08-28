using Microsoft.AspNetCore.Http;

namespace WebApiFacebookOAuth.Services
{
    public class UtilService
    {
        readonly HttpContext _context;
        public UtilService(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor.HttpContext;
        }

        public string GetBaseUri() => $"{_context.Request.Scheme}://{_context.Request.Host}";
    }
}