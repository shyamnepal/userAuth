

using LoginSystemView.Model.ApyTypeEnum;

namespace LoginSystemView.Model
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string ApiUrl { get; set; }
        public object Data { get; set; }
    }
}
