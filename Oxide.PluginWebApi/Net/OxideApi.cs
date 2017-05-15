using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Oxide.PluginWebApi.Net
{
    public class OxideApi : IDisposable
    {
        private static readonly CookieContainer cookieContainer = new CookieContainer();

        private readonly CookieWebClient webClient;

        public static bool Authenticate(string username, string password)
        {
            

            return false;
        }

        public OxideApi()
        {
            webClient = new CookieWebClient(cookieContainer);
        }

        public void Dispose()
        {
            webClient.Dispose();
        }


    }
}