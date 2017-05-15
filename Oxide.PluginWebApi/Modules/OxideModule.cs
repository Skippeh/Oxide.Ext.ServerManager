using Oxide.PluginWebApi.Api;

namespace Oxide.PluginWebApi.Modules
{
    public class OxideModule : BaseModule
    {
        public OxideModule() : base("oxide")
        {
            Get["lastupdate"] = WrapMethod(_ => GetLastUpdate());
            Get["plugin/{resourceId:int}"] = WrapMethod((dynamic _) => GetPlugin(_.resourceId));
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
            

            return null;
        }
    }
}