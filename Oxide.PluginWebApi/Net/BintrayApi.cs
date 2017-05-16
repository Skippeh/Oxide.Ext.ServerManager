using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Oxide.PluginWebApi.Net.Models.Bintray;
using Version = Oxide.PluginWebApi.Net.Models.Bintray.Version;

namespace Oxide.PluginWebApi.Net
{
    public class BintrayApi : BaseApi
    {
        private const string ApiUrl = "https://api.bintray.com/";
        private const string FilesPath = "packages/oxidemod/builds/Oxide/files";
        private const string VersionsPath = "packages/oxidemod/builds/Oxide/versions/_latest";
        
        public BintrayApi(string username, string apiKey)
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + apiKey));
            WebClient.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";
        }

        public Version GetVersion()
        {
            HttpWebResponse error;
            return JsonConvert.DeserializeObject<Version>(DownloadString(ApiUrl + VersionsPath, out error));
        }

        public File[] GetFiles()
        {
            HttpWebResponse error;
            return JsonConvert.DeserializeObject<File[]>(DownloadString(ApiUrl + FilesPath, out error));
        }
    }
}