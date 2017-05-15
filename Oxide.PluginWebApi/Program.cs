using System;
using Oxide.PluginWebApi.Configuration;

namespace Oxide.PluginWebApi
{
    internal class Program
    {
        public static ApiConfig Config { get; private set; }

        static void Main(string[] args)
        {
            Config = ConfigManager<ApiConfig>.LoadOrCreate();
        }
    }
}