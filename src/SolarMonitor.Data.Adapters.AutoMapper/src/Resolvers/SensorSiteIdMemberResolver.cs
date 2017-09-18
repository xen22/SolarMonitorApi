using AutoMapper;
using SolarMonitor.Data.Repositories;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class SensorSiteIdMemberResolver : IValueResolver<Resources.Sensor, Models.Sensor, int>
    {
        private ISiteRepository _siteRepository;
        public SensorSiteIdMemberResolver(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository ?? throw new System.ArgumentNullException(nameof(siteRepository));
        }

        public int Resolve(Resources.Sensor source, Models.Sensor destination,
            int destMember, ResolutionContext context)
        {
            return _siteRepository.SiteByName(source.Site).Id;
        }
    }
}
