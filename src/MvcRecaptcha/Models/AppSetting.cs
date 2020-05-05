namespace MvcRecaptcha.Models
{
    public class GoogleReCaptchaSetting
    {
        public string SiteKey { get; set; }
        public string SecretKey { get; set; }
    }

    public class AppSetting
    {
        public GoogleReCaptchaSetting GoogleReCaptcha { get; set; }
    }
}