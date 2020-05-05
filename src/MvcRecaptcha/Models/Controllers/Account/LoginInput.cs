using System.ComponentModel.DataAnnotations;

namespace MvcRecaptcha.Models.Controllers.Account
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}