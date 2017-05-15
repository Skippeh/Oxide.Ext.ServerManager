using Newtonsoft.Json;
using Oxide.PluginWebApi.Configuration;

namespace Oxide.PluginWebApi
{
    public class ApiConfig : ConfigFile
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OxideUsername { get; set; } = "";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OxidePassword { get; set; } = "";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BintrayUsername { get; set; } = "";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BintrayApiKey { get; set; } = "";

        public int Port { get; set; } = 8090;
    }
}