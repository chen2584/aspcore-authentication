using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiJwtRsa.Helpers;
using WebApiJwtRsa.Models;

namespace WebApiJwtRsa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly IConfiguration configuration;
        readonly IHostingEnvironment hostingEnvironment;
        public AccountController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<Dictionary<string, string>> Get()
        {
            var claimsDic = new Dictionary<string, string>();
            foreach (var claim in User.Claims)
            {
                claimsDic[claim.Type] = claim.Value;
            }
            return claimsDic;
        }

        private (DateTime expires, string accessTolen) GetToken(string username)
        {
            var utcNow = DateTime.UtcNow;

            var privateKeyPath = Path.Combine(hostingEnvironment.ContentRootPath, "ssh", configuration["Tokens:PrivateKey"]);
            var privateRsa = RsaHelper.PrivateKeyFromPemFile(privateKeyPath);
            var privateKey = new RsaSecurityKey(privateRsa);
            SigningCredentials signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "1"),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, $"{username}@localhost.con"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var expires = utcNow.AddSeconds(this.configuration.GetValue<int>("Tokens:Lifetime"));

            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: expires,
                audience: this.configuration.GetValue<String>("Tokens:Audience"),
                issuer: this.configuration.GetValue<String>("Tokens:Issuer")
                );

            return (expires, new JwtSecurityTokenHandler().WriteToken(jwt));
        }

        [HttpPost]
        public ActionResult<UserLoginOutput> Login(UserLoginInput input)
        {
            if (input.Username.Equals("admin") && input.Password.Equals("admin"))
            {
                var (expire, accessToken) = GetToken(input.Username);
                var output = new UserLoginOutput
                {
                    AccessToken = accessToken,
                    Expire = expire
                };
                return output;
            }
            return Unauthorized();
        }
    }
}
