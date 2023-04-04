using LoginSystemView.Model;
using LoginSystemView.Models;

namespace LoginSystemView.Services.IServices
{
    public interface IBaseServices
    {
        Response responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
