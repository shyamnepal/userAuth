using System.ComponentModel.DataAnnotations;

namespace LoginSystem.Model
{
    public class ChangeUserPasswordModel
    {
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new Password")]
        [Compare("NewPassword", ErrorMessage ="The password Does not match")]
        public string ConfirmNewPassword { get; set; }


    }
}
