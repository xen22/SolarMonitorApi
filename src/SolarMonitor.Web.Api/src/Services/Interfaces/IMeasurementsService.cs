using System;
using SolarMonitorApi.RequestQueries;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitorApi.Services
{
    public interface IMeasurementsService
    {
        Resources.CollectionResource<Resources.Measurement> Get(GetMeasurementsRequestQuery query);
    }
}
