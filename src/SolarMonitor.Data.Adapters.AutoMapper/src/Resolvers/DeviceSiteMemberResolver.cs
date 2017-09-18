using AutoMapper;
using SolarMonitor.Data.Repositories;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class DeviceSiteMemberResolver : IValueResolver<Resources.Device, Models.Device, Models.Site>
    {
        private ISiteRepository _siteRepository;
        public DeviceSiteMemberResolver(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository ?? throw new System.ArgumentNullException(nameof(siteRepository));
        }

        public Models.Site Resolve(Resources.Device source, Models.Device destination,
            Models.Site destMember, ResolutionContext context)
        {
            return _siteRepository.SiteByName(source.SiteName);
        }
    }
}
