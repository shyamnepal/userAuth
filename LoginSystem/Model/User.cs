using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LoginSystem.Model
{
    public class User : IdentityUser
    {
        
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }




       
        


    }
}
