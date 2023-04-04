using System.ComponentModel.DataAnnotations;

namespace LoginSystemView.Models
{
    public class UserProfile
    {
        [Required(ErrorMessage ="city is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image { get; set; }
        public string? imgurl { get; set; } = null;
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Education is required")]
        public string Education { get; set; }

    }
}
