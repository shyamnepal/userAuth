namespace LoginSystem.Model
{
    public class UserProfileDto
    {
        public string City { get; set; }
        public string UserId { get; set; }
        public IFormFile Image { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string gender { get; set; }
        public string Education { get; set; }
    }
}
