using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiFacebookOAuth.Entities;
using WebApiFacebookOAuth.Models;

namespace WebApiFacebookOAuth.Services
{
    public class AuthenticationService
    {
        readonly MyDbContext _db;
        public AuthenticationService(MyDbContext db)
        {
            _db = db;
        }

        public async Task AddFacebookUserIfNotExistAsync(OAuthUserInfo userInfo)
        {
            var hasUser = await _db.FacebookAccounts.AnyAsync(x => x.FacebookId.Equals(userInfo.Id));
            if (!hasUser)
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
            }
        }
    }
}