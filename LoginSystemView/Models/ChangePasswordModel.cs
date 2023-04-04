using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LoginSystemView.Models
{
    public class ChangePasswordModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage ="Current Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new Password")]
        [Compare("NewPassword", ErrorMessage = "The password Does not match")]
        public string? ConfirmNewPassword { get; set; } = null;
    }
}
