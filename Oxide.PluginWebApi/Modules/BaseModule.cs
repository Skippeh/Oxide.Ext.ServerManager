using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Oxide.PluginWebApi.Api;
using Oxide.PluginWebApi.Net;
using HttpStatusCode = Nancy.HttpStatusCode;

namespace Oxide.PluginWebApi.Modules
{
    public abstract class BaseModule : NancyModule
    {
        protected BintrayApi CreateBintrayApi() => new BintrayApi(Config.BintrayUsername, Config.BintrayApiKey);
        protected OxideApi CreateOxideApi() => new OxideApi();

        protected ApiConfig Config => Program.Config;

        protected BaseModule() { }
        protected BaseModule(string modulePath) : base(modulePath) { }

        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        protected Func<dynamic, dynamic> WrapMethod(Func<dynamic, ApiResponse> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            return (dynamic parameters) =>
            {
                ApiResponse response;

                try
                {
                    response = func(parameters);
                }
                catch (ApiResponseException ex)
                {
                    response = new ApiResponse(null, ex.StatusCode, ex.Message);
                }
                catch (Exception ex)
                {
                    response = new ApiResponse(null, HttpStatusCode.InternalServerError, ex.ToString());
                }

                return ResponseFromResult(response);
            };
        }

        private Response ResponseFromResult(ApiResponse result)
        {
            return new Response
            {
                StatusCode = result?.StatusCode ?? HttpStatusCode.OK,
                Contents = stream =>
                {
                    if (result != null)
                    {
                        var response = result.StatusCode == HttpStatusCode.OK ? result.Data : result;
                        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response, serializerSettings));
                        stream.Write(bytes, 0, bytes.Length);
                    }
                },
                ContentType = "application/json"
            };
        }
    }
}