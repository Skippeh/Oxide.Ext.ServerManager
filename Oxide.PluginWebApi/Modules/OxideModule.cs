using Nancy;
using Oxide.PluginWebApi.Api;
using Oxide.PluginWebApi.Net.Models.Oxide;

namespace Oxide.PluginWebApi.Modules
{
    public class OxideModule : BaseModule
    {
        public OxideModule() : base("oxide")
        {
            Get["lastupdate"] = WrapMethod(_ => GetLastUpdate());
            Get["plugin/{resourceId:int}"] = WrapMethod((dynamic _) => GetPlugin(_.resourceId));
            Get["plugin/{resourceId:int}/download/{version:int}"] = WrapMethod((dynamic _) => DownloadPlugin(_.resourceId, _.version));
        }

        private ApiResponse GetLastUpdate()
        {
            using (var bintray = CreateBintrayApi())
            {
                return new ApiResponse(new
                {
                    LastUpdate = bintray.GetVersion().Updated
                });
            }
        }

        private ApiResponse GetPlugin(int resourceId)
        {
            using (var oxide = CreateOxideApi())
            {
                Plugin plugin = oxide.GetPlugin(resourceId);

                if (plugin == null)
                    throw new ApiResponseException(HttpStatusCode.NotFound, "Could not find a plugin with resource id: " + resourceId);

                return new ApiResponse(plugin);
            }
        }

        private ApiResponse DownloadPlugin(int resourceId, int version)
        {
            using (var oxide = CreateOxideApi())
            {
                return new ApiResponse(new
                {
                    script = oxide.DownloadPlugin(resourceId, version)
                });
            }
        }
    }
}