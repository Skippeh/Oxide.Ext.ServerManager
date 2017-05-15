using Oxide.PluginWebApi.Configuration;

namespace Oxide.PluginWebApi
{
    public class ApiConfig : ConfigFile
    {
        public string OxideUsername { get; set; } = "";
        public string OxidePassword { get; set; } = "";
        public int Port { get; set; } = 8090;
    }
}