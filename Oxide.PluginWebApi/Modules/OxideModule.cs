using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Oxide.PluginWebApi.Api;

namespace Oxide.PluginWebApi.Modules
{
    public class OxideModule : BaseModule
    {
        public OxideModule()
        {
            Get["versions"] = (dynamic _) => GetVersions();
        }

        private ApiResponse GetVersions()
        {
            // Example response, todo: implement
            return new ApiResponse(new
            {
                Oxide = "1.0.0",
                Games = new
                {
                    Rust = "1.0.0",
                    Terraria = "1.0.0"
                }
            });
        }
    }
}