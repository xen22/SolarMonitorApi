using System;
using AD.Common;
using SolarMonitor.Data.CommonTypes;
using Models = SolarMonitor.Data.Models;

namespace SolarMonitor.Api.IntegrationTests
{
    public class SensorFactory : GenericFactory<SensorType, Models.Sensor>
    {
        public SensorFactory()
        : base()
        {
        }

        public Models.Sensor CreateSensor(SensorType type, int id, Models.Site site, Models.SensorType stype)
        {
            var s = CreateObject(type);

            s.Id = id;
            s.SiteId = site.Id;
            s.Site = site;
            s.TypeId = (int)type;
            s.Type = stype;
            s.Name = $"{type.ToString()}{id}";
            s.Guid = Guid.NewGuid();
            s.Device = Guid.NewGuid();
            s.Description = $"Description{id}";
            s.Manufacturer = $"Manufacturer{id}";
            s.Model = $"Model{id}";
            s.DetailedSpecs = $"DetailedSpecs{id}";

            // configure other properties
            if (s.GetType() == typeof(Models.CurrentSensor))
            {


            }
            else if (s.GetType() == typeof(Models.TemperatureSensor))
            {

            }

            return s;
        }
    }

}