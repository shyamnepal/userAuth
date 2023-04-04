using LoginSystemView.Model;
using LoginSystemView.Model.ApyTypeEnum;
using LoginSystemView.Models;
using Newtonsoft.Json;
using System.Text;

namespace LoginSystemView.Services.IServices
{
    public class BaseServices : IBaseServices
    {
        public Response responseModel { get; set; }    
        public IHttpClientFactory httpClient { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseServices(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                
                var client = httpClient.CreateClient("Api");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.ApiUrl);
               
                if (apiRequest.Data != null)
                {
                    if(apiRequest.Data.GetType()!= typeof(MultipartFormDataContent))
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8, "application/json");
                    }
                      
                    
                  
                        
                }
                switch (apiRequest.ApiType)
                {
                    case ApiType.POST:
                        message.Method=HttpMethod.Post;
                        break;
                      case ApiType.PUT:
                        message.Method=HttpMethod.Put;
                        break;
                      case ApiType.DELETE:
                        message.Method=HttpMethod.Delete;
                        break;
                    default:
                        message.Method=HttpMethod.Get;
                        break;

                }

                HttpResponseMessage apiResponse = null;
                if (_httpContextAccessor.HttpContext.Session.GetString("token") != null)
                {
                     client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("token").ToString());

                }
                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception ex)
            {
                var APIResponse = JsonConvert.DeserializeObject<T>(ex.Message);
                return APIResponse;
            }
            
        }
    }
}
