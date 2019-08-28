using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiFacebookOAuth.Entities;
using WebApiFacebookOAuth.Models;

namespace WebApiFacebookOAuth.Services
{
    public class AuthenticationService
    {
        readonly AppSetting _setting;
        readonly MyDbContext _db;
        public AuthenticationService(AppSetting setting, MyDbContext db)
        {
            _setting = setting;
            _db = db;
        }

        private async Task<FacebookAccountEntity> GetFacebookAccountAsync(string facebookId)
        {
            var facebookAccount = await _db.FacebookAccounts.Include(x => x.UserProfile).FirstOrDefaultAsync(x => x.FacebookId.Equals(facebookId));
            return facebookAccount;
        }

        private async Task<FacebookAccountEntity> CreateFacebookUserProfile(FacebookOAuthUserInfo userInfo)
        {
            var facebookAccount = new FacebookAccountEntity
            {
                FacebookId = userInfo.Id,
                UserProfile = new UserProfilEntity
                {
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    Email = userInfo.Email
                }
            };
            _db.FacebookAccounts.Add(facebookAccount);
            await _db.SaveChangesAsync();
            return facebookAccount;
        }

        public async Task<FacebookAccountEntity> GetUserProfileAsync(FacebookOAuthUserInfo userInfo)
        {
            var facebookAccount = await GetFacebookAccountAsync(userInfo.Id);
            if (facebookAccount == null)
            {
                facebookAccount = await CreateFacebookUserProfile(userInfo);
            }
            return facebookAccount;
        }

        private (string token, DateTime expires) GetToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.JwtAuthentication.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(20);

            var jwtToken = new JwtSecurityToken(
                issuer: _setting.JwtAuthentication.Issuer,
                audience: _setting.JwtAuthentication.Audience,
                signingCredentials: credentials,
                expires: expires,
                claims: claims
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (token, expires);
        }

        private (string token, DateTime expires) GenerateAuthorizationToken(int id, string firstName = null, string lastName = null, string email = null)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
            if (!string.IsNullOrEmpty(firstName))
            {
                claims.Add(new Claim(ClaimTypes.GivenName, firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                claims.Add(new Claim(ClaimTypes.Surname, lastName));
            }

            if (!string.IsNullOrEmpty(email))
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
            }
            return GetToken(claims);
        }

        public (string token, DateTime expires) GenerateAuthorizationToken(FacebookAccountEntity facebookAccount)
        {
            var userProfile = facebookAccount.UserProfile;
            return GenerateAuthorizationToken(userProfile.Id, userProfile.FirstName, userProfile.LastName, userProfile.Email);
        }
    }
}