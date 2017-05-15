using System;

namespace Oxide.PluginWebApi.Net.Models.Bintray
{
    public class Version
    {
        public string Name { get; set; }
        public string Package { get; set; }
        public string Repo { get; set; }
        public string Owner { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}