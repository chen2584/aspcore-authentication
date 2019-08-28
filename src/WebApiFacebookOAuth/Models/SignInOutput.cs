using System;

namespace WebApiFacebookOAuth.Models
{
    public class SignInOutput
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresDate { get; set; }
    }
}