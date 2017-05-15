using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Oxide.PluginWebApi.Net;

namespace Oxide.PluginWebApi.Modules
{
    public abstract class BaseModule : NancyModule
    {
        protected CookieWebClient CreateWebClient() => new CookieWebClient(Program.CookieContainer);
        protected ApiConfig Config => Program.Config;
    }
}