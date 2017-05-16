using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Oxide.PluginWebApi.Net
{
    public class OxideApi : IDisposable
    {
        private static readonly CookieContainer cookieContainer = new CookieContainer();

        private readonly CookieWebClient webClient;

        public static bool Authenticate(string username, string password)
        {
            using (var webClient = new CookieWebClient(cookieContainer))
            {
                byte[] responseBytes = webClient.UploadValues("http://oxidemod.org/login/login", "POST", new NameValueCollection
                {
                    { "login", username },
                    { "password", password }
                });

                string response = Encoding.UTF8.GetString(responseBytes);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response);
                var rootNode = htmlDoc.DocumentNode;
                
                var content = rootNode.SelectSingleNode("//*[@id=\"content\"]");

                if (content.GetAttributeValue("class", "").Contains("error_with_login"))
                {
                    return false;
                }

                return true;
            }
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