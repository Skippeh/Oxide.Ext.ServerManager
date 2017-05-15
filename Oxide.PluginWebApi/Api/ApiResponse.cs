using Nancy;
using Newtonsoft.Json;

namespace Oxide.PluginWebApi.Api
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] // Don't include in response if it's null.
        public object Data { get; set; }

        public ApiResponse(object data)
        {
            Data = data;
        }

        public ApiResponse(HttpStatusCode statusCode, object data = null)
        {
            StatusCode = statusCode;
            Data = data;
        }
    }
}