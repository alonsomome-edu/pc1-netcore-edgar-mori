using Newtonsoft.Json;

namespace Banking.Net.Common.Infra.API
{
    public class ApiStringResponse
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        public ApiStringResponse(string message)
        {
            Message = message;
        }
    }
}