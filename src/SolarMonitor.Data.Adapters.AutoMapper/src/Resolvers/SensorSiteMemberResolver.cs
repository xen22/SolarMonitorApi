using AutoMapper;
using SolarMonitor.Data.Repositories;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class SensorSiteMemberResolver : IValueResolver<Resources.Sensor, Models.Sensor, Models.Site>
    {
        private ISiteRepository _siteRepository;
        public SensorSiteMemberResolver(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository ?? throw new System.ArgumentNullException(nameof(siteRepository));
        }

        public Models.Site Resolve(Resources.Sensor source, Models.Sensor destination,
            Models.Site destMember, ResolutionContext context)
        {
            return _siteRepository.SiteByName(source.Site);
        }
    }
}
