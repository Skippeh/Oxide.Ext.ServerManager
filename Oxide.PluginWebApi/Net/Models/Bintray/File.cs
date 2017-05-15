using System;

namespace Oxide.PluginWebApi.Net.Models.Bintray
{
    public class File
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Repo { get; set; }
        public string Package { get; set; }
        public string Version { get; set; }
        public string Owner { get; set; }
        public DateTime Created { get; set; }
        public int Size { get; set; }
        public string Sha1 { get; set; }
        public string Sha256 { get; set; }
    }
}