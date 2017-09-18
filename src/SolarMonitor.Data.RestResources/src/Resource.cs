using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SolarMonitor.Data.Resources
{
    public abstract class Resource : IResource
    {
        [JsonProperty(PropertyName = "_links")]
        public IDictionary<string, Link> Links { get; protected set; }

        public bool ShouldSerializeLinks()
        {
            return (Links != null) && (Links.Count > 0);
        }

        string _resourceName;

        public Resource(string resourceName)
        {
            _resourceName = resourceName;
            Links = new Dictionary<string, Link>();
        }

        public void AddLink(string name, Link link)
        {
            Links[name] = link;
        }

        public abstract void GenerateLinks();
    }
}
