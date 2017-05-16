using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Text;

namespace Oxide.PluginWebApi.Net
{
    public abstract class BaseApi : IDisposable
    {
        protected CookieWebClient WebClient { get; private set; }

        private static readonly CookieContainer cookieContainer = new CookieContainer();

        protected BaseApi()
        {
            WebClient = new CookieWebClient(cookieContainer);
        }

        public void Dispose()
        {
            WebClient.Dispose();
        }
        
        #region Static helper methods

        protected static string DownloadString(CookieWebClient webClient, string url, out HttpWebResponse errorResponse)
        {
            return TryAction(() => webClient.DownloadString(url), out errorResponse);
        }

        protected static string PostString(CookieWebClient webClient, string url, NameValueCollection values, out HttpWebResponse errorResponse)
        {
            return TryAction(() =>
            {
                byte[] bytes = webClient.UploadValues(url, "POST", values);
                return Encoding.UTF8.GetString(bytes);
            }, out errorResponse);
        }

        #endregion

        protected string DownloadString(string url, out HttpWebResponse errorResponse)
        {
            return DownloadString(WebClient, url, out errorResponse);
        }

        protected string PostString(string url, NameValueCollection values, out HttpWebResponse errorResponse)
        {
            return PostString(WebClient, url, values, out errorResponse);
        }

        private static T TryAction<T>(Func<T> func, out HttpWebResponse errorResponse)
        {
            try
            {
                errorResponse = null;
                return func();
            }
            catch (WebException ex)
            {
                if (!(ex.Response is HttpWebResponse))
                    throw;

                errorResponse = ex.Response as HttpWebResponse;
                return default(T);
            }
        }
    }
}