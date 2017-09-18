using System.Collections.Generic;

namespace SolarMonitor.Data.Resources
{
    public interface ICollectionResource
    {
        int PageIndex { get; set; }
        int CountSize { get; set; }

        int TotalCount { get; set; }
        int TotalPageCount { get; set; }

        bool HasNextPage { get; set; }
        bool HasPreviousPage { get; set; }

        ICollection<IResource> Items { get; }
        IDictionary<string, Link> Links { get; set; }
    }
}
