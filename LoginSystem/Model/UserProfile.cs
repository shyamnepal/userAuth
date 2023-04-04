using System.ComponentModel.DataAnnotations;

namespace LoginSystem.Model
{
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }
        public string City { get; set; }
        public string imgurl { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Education { get; set; }

        public string UserId { get; set; }
    
        public User AppUser { get; set; }

        
    }
}
