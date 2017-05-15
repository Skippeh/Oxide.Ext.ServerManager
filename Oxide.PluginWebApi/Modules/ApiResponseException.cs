using System;
using Nancy;

namespace Oxide.PluginWebApi.Modules
{
    public sealed class ApiResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ApiResponseException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}