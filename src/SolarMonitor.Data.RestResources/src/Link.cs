using Newtonsoft.Json;
using System;

namespace SolarMonitor.Data.Resources
{
    public class Link : IEquatable<Link>
    {
        string _href;

        [JsonProperty(PropertyName = "href")]
        public string HRef
        {
            get { return _href; }
            set { _href = value; }
        }

        public Link(string href)
        {
            _href = href;
        }

        public bool Equals(Link other)
        {
            return (other != null &&
                    _href == other._href);
        }
    }
}
