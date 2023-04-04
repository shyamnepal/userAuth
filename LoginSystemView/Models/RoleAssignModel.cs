using System.ComponentModel.DataAnnotations;

namespace LoginSystemView.Models
{
    public class RoleAssignModel
    {
     
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
