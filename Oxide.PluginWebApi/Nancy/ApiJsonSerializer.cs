using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Oxide.PluginWebApi.Nancy
{
    public sealed class ApiJsonSerializer : JsonSerializer
    {
        public ApiJsonSerializer()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}