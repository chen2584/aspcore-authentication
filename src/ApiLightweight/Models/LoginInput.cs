using System.ComponentModel.DataAnnotations;

namespace ApiLightweight.Models
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}