using AutoMapper;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public class SiteProfile : Profile
    {
        public SiteProfile()
        {
            // models => resources
            CreateMap<Models.Site, Resources.Site>()
                .AfterMap((model, resource) => resource.GenerateLinks());

            // resources => models
            CreateMap<Resources.Site, Models.Site>();

        }
    }
}
