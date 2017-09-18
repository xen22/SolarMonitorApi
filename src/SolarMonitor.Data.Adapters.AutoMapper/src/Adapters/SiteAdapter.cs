using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using AutoMapper;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class SiteAdapter : GenericAdapter<Models.Site, Resources.Site>, ISiteAdapter
    {
        public SiteAdapter(IMapper mapper) : base(mapper)
        {
        }
    }
}