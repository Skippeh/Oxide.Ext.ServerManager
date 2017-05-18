using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Nancy.Helpers;
using Oxide.PluginWebApi.Modules;
using Oxide.PluginWebApi.Net.Models.Oxide;

namespace Oxide.PluginWebApi.Net
{
    public class OxideApi : BaseApi
    {
        private const string PluginUrl = "http://oxidemod.org/plugins/{0}/history";
        
        public static bool Authenticate(string username, string password)
        {
            using (var webClient = new CookieWebClient(CookieContainer))
            {
                HttpWebResponse error;
                string response = PostString(webClient, "http://oxidemod.org/login/login", new NameValueCollection
                {
                    { "login", username },
                    { "password", password }
                }, out error);

                if (error != null)
                {
                    throw new ApiResponseException(global::Nancy.HttpStatusCode.InternalServerError, error.StatusDescription);
                }

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response);
                
                var rootNode = htmlDoc.DocumentNode;
                var accountMenu = rootNode.SelectSingleNode("//*[@id=\"AccountMenu\"]");
                
                return accountMenu != null;
            }
        }

        public Plugin GetPlugin(int resourceId)
        {
            HttpWebResponse error;
            string response = DownloadString(string.Format(PluginUrl, resourceId), out error);

            if (error?.StatusCode == HttpStatusCode.Forbidden)
            {
                return null; // Plugin not found or not allowed access.
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var rootNode = htmlDoc.DocumentNode;
            var result = new Plugin();
            var resourceDesc = rootNode.SelectSingleNode("//*[@class='mainContent']/div[@class='resourceInfo']/div[@class='resourceDesc']");

            result.Name = ParsePluginName(resourceDesc);
            result.Game = ParsePluginGame(resourceDesc);

            result.Description = resourceDesc.SelectSingleNode("p").InnerText;

            var historyTable = rootNode.SelectSingleNode("//*[@class='dataTable resourceHistory']");

            foreach (var rowNode in historyTable.SelectNodes("tr").Skip(1)) // Skip header
            {
                Plugin.Version item = ParsePluginRow(rowNode);
                result.Versions.Add(item);
            }

            return result;
        }

        private static Plugin.Version ParsePluginRow(HtmlNode rowNode)
        {
            Plugin.Version result = new Plugin.Version();

            result.Value = rowNode.SelectSingleNode("td[1]").InnerText;

            HtmlNode dateTimeNode = rowNode.SelectSingleNode("td[2]/*[@class='DateTime']");
            string dateString = dateTimeNode.GetAttributeValue("title", dateTimeNode.InnerText);
            result.ReleaseDate = DateTime.ParseExact(dateString, "MMM d', 'yyyy' at 'h:mm' 'tt", CultureInfo.InvariantCulture).ToUniversalTime();

            string downloadsString = rowNode.SelectSingleNode("td[3]").InnerText;
            result.Downloads = int.Parse(downloadsString, NumberStyles.AllowThousands, CultureInfo.InvariantCulture);

            string downloadHref = rowNode.SelectSingleNode("td[4]/a").GetAttributeValue("href", null);
            string versionStr = HttpUtility.ParseQueryString(downloadHref.Split('?')[1])["version"];
            result.Id = int.Parse(versionStr);

            return result;
        }

        private string ParsePluginName(HtmlNode resourceDesc)
        {
            string title = resourceDesc.SelectSingleNode("h1").InnerText.Trim();

            if (!title.ToLower().Contains(" for ")) // Universal plugins don't have the "for Game" suffix.
                return title;

            var regex = new Regex(@"(.+)\sfor\s", RegexOptions.IgnoreCase);
            return regex.Split(title)[1];
        }

        private string ParsePluginGame(HtmlNode resourceDesc)
        {
            string title = resourceDesc.SelectSingleNode("h1").InnerText.Trim();

            if (!title.ToLower().Contains(" for "))
                return null;

            var regex = new Regex(@".+\sfor\s(.+)", RegexOptions.IgnoreCase);
            return regex.Split(title)[1];
        }
    }
}