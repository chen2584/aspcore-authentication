using System.ComponentModel.DataAnnotations;

namespace WebApiFacebookOAuth.Entities
{
    public class UserProfilEntity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
    }
}