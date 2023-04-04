namespace LoginSystem.Model
{
    public class response
    {
        public int statusCode { get; set; }
        
        public string token { get; set; }
        public string userId { get; set; }
        public string userEmail { get; set; }

    }
}
