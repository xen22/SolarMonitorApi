using System.Collections.Generic;
using Newtonsoft.Json;

namespace SolarMonitor.Data.Resources
{
    public class CollectionResource<ResourceType>
    {
        string _resourcePrefix;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPageCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

        [JsonProperty(PropertyName = "_links")]
        public IDictionary<string, Link> Links { get; set; }

        [JsonProperty(PropertyName = "_embedded")]
        public ICollection<ResourceType> Items { get; }

        public CollectionResource(string resourcePrefix, int pageIndex, int pageSize, int totalCount, ICollection<ResourceType> items = null)
        {
            _resourcePrefix = resourcePrefix;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = totalCount / pageSize + 1;
            HasNextPage = pageIndex < TotalPageCount;
            HasPreviousPage = pageIndex > 1;

            const string pageIndexParam = "pageIndex";
            const string pageSizeParam = "pageSize";

            Links = new Dictionary<string, Link>();
            Links["self"] = new Link($"{_resourcePrefix}?{pageIndexParam}={PageIndex}&{pageSizeParam}={PageSize}");
            Links["first"] = new Link($"{_resourcePrefix}?{pageIndexParam}=1&{pageSizeParam}={PageSize}");
            Links["last"] = new Link($"{_resourcePrefix}?{pageIndexParam}={TotalPageCount}&{pageSizeParam}={PageSize}");

            if (HasNextPage)
            {
                Links["next"] = new Link($"{_resourcePrefix}?{pageIndexParam}={PageIndex + 1}&{pageSizeParam}={PageSize}");
            }
            if (HasPreviousPage)
            {
                Links["previous"] = new Link($"{_resourcePrefix}?{pageIndexParam}={PageIndex - 1}&{pageSizeParam}={PageSize}");
            }
            Items = items ?? new List<ResourceType>();
        }

        public void AddLink(string name, Link link)
        {
            Links[name] = link;
        }

        public void AddItem(ResourceType item)
        {
            Items.Add(item);
        }
    }
}
