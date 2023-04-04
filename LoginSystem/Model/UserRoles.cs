namespace LoginSystem.Model
{
    public class UserRoles
    {
        public string Id { get; set; }
        public string Username { get; set; }
       public string Email { get; set; }
        public string? Role { get; set; } = null;
        public string RoleId { get; set; }


    }
}
