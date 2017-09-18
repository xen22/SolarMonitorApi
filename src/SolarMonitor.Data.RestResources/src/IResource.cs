using System.Collections.Generic;

namespace SolarMonitor.Data.Resources
{
    public interface IResource
    {
        IDictionary<string, Link> Links { get; }
        void GenerateLinks();
    }
}
