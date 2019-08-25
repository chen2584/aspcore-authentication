using System;

namespace WebApiJwtRsa.Models
{
    public class UserLoginOutput
    {
        public string AccessToken { get; set; }
        public DateTime Expire{ get; set; }
    }
}