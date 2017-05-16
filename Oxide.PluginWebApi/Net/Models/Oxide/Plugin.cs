using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxide.PluginWebApi.Net.Models.Oxide
{
    public class Plugin
    {
        public class Version
        {
            public int Id { get; set; }
            public string Value { get; set; }
            public int Downloads { get; set; }
            public DateTime ReleaseDate { get; set; }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<Version> Versions { get; set; } = new List<Version>();
    }
}