using System;
using System.Net;
using Nancy.Hosting.Self;
using Oxide.PluginWebApi.Configuration;
using Oxide.PluginWebApi.Nancy;
using Oxide.PluginWebApi.Net;

namespace Oxide.PluginWebApi
{
    internal class Program
    {
        public static ApiConfig Config { get; private set; }
        public static CookieContainer CookieContainer { get; private set; }

        static void Main(string[] args)
        {
            Config = ConfigManager<ApiConfig>.LoadOrCreate();
            CookieContainer = new CookieContainer();

            using (var host = new NancyHost(new ApiBootstrapper(), new HostConfiguration
            {
                UrlReservations = new UrlReservations
                {
                    CreateAutomatically = true
                }
            }, new Uri($"http://localhost:{Config.Port}")))
            {
                Console.WriteLine($"Starting api server on port {Config.Port}...");
                host.Start();

                if (!OxideApi.Authenticate(Config.OxideUsername, Config.OxidePassword))
                {
                    Console.Error.WriteLine("Failed to authenticate with Oxide.");
                    return;
                }

                Console.WriteLine("Press CTRL+Q to stop the server.");
                ConsoleKeyInfo consoleKeyInfo;
                while ((consoleKeyInfo = Console.ReadKey(true)).Key != ConsoleKey.Q || consoleKeyInfo.Modifiers != ConsoleModifiers.Control)
                    continue;

                Console.WriteLine("Stopping api server...");
                host.Stop();
            }
        }
    }
}