namespace WebApiFacebookOAuth.Models
{
    public class AppSetting
    {
        public string ConnectionString { get; set; }
        public AuthenticationSetting Authentication { get; set; }
        public JwtAuthenticationSetting JwtAuthentication { get; set; }
    }

    public class AuthenticationSetting
    {
        public FacebookAuthentiactionSetting Facebook { get; set; }
    }

    public class JwtAuthenticationSetting
    {
        public string SecurityKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }

    public class FacebookAuthentiactionSetting
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}