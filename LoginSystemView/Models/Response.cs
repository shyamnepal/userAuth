using System.Net;

namespace LoginSystemView.Models
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
