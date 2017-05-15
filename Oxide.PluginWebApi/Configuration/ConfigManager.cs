using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Oxide.PluginWebApi.Configuration
{
    public static class ConfigManager<T> where T : ConfigFile
    {
        // ReSharper disable once StaticMemberInGenericType
        public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };

        public static T LoadOrCreate() { return LoadOrCreate("config.json"); }
        public static T LoadOrCreate(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
            {
                return CreateDefaultConfig(filePath);
            }

            T loaded = LoadConfig(filePath);
            SaveConfig(loaded, filePath); // Update existing config with potential new or deleted config values.
            return loaded;
        }

        public static T CreateDefaultConfig() { return CreateDefaultConfig("config.json"); }
        public static T CreateDefaultConfig(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            if (File.Exists(filePath))
            {
                File.Move(filePath, filePath + ".old");
            }

            T defaultConfig = Activator.CreateInstance<T>();

            using (var file = File.CreateText(filePath))
            {
                file.Write(JsonConvert.SerializeObject(defaultConfig, SerializerSettings));
            }

            return defaultConfig;
        }

        public static T LoadConfig() { return LoadConfig("config.json"); }
        public static T LoadConfig(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json, SerializerSettings);
        }

        public static bool SaveConfig(T config) { return SaveConfig(config, "config.json"); }
        public static bool SaveConfig(T config, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(config, SerializerSettings));
                return true;
            }
            catch (IOException ex)
            {
                return false;
            }
        }
    }
}