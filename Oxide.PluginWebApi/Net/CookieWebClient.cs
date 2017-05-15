using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Oxide.PluginWebApi.Net
{
    public class CookieWebClient : WebClient
    {
        public readonly CookieContainer CookieContainer;

        public CookieWebClient(CookieContainer cookieContainer)
        {
            CookieContainer = cookieContainer;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            var webRequest = request as HttpWebRequest;

            if (webRequest != null)
            {
                webRequest.CookieContainer = CookieContainer;
            }

            return request;
        }
    }
}