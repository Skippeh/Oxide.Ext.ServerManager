using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Oxide.PluginWebApi.Net.Models.Bintray;

namespace Oxide.PluginWebApi.Net
{
    public class BintrayApi : IDisposable
    {
        private const string ApiUrl = "https://api.bintray.com/";
        private const string FilesPath = "packages/oxidemod/builds/Oxide/files";
        
        private readonly WebClient webClient; // Don't need cookies for this

        public BintrayApi(string username, string apiKey)
        {
            webClient = new WebClient();
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + apiKey));
            webClient.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";
        }

        public void Dispose()
        {
            webClient.Dispose();
        }

        public async Task<File[]> GetFiles()
        {
            return JsonConvert.DeserializeObject<File[]>(await webClient.DownloadStringTaskAsync(ApiUrl + FilesPath));
        }
    }
}