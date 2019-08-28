using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiFacebookOAuth.Entities
{
    public class FacebookAccountEntity
    {
        [Key]
        public int UserProfileId { get; set; }
        [Required]
        [StringLength(30)]
        public string FacebookId { get; set; }
        [ForeignKey(nameof(UserProfileId))]
        public UserProfilEntity UserProfile { get; set; }
    }
}