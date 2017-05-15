using Nancy;
using Nancy.Bootstrapper;
using Nancy.Diagnostics;
using Nancy.TinyIoc;
using Newtonsoft.Json;

#if !DEBUG
using Nancy.Diagnostics;
#endif

namespace Oxide.PluginWebApi.Nancy
{
    public class ApiBootstrapper : DefaultNancyBootstrapper
    {
#if DEBUG
        protected override DiagnosticsConfiguration DiagnosticsConfiguration => new DiagnosticsConfiguration
        {
            Password = "debug"
        };
#endif

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
#if !DEBUG
            // Disable /_nancy diagnostics page.
            DiagnosticsHook.Disable(pipelines);
#endif

            StaticConfiguration.DisableErrorTraces = false;

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                            .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                            .WithHeader("Access-Control-Allow-Headers", "Accept,Origin,Content-Type");
            });
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<JsonSerializer, ApiJsonSerializer>();
        }
    }
}
