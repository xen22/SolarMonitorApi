using System;
using System.Reflection;
using AD.Common;
using SolarMonitor.Data.CommonTypes;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitor.Api.IntegrationTests
{
    public class ResourceSensorFactory : GenericFactory<SensorType, Resources.Sensor>
    {
        public ResourceSensorFactory()
        : base()
        {
        }

        public Resources.Sensor CreateSensor(SensorType type, int id, string siteName)
        {
            var s = CreateObject(type);

            s.Site = siteName;
            s.Type = type.ToString();
            s.Name = $"{type.ToString()}{id}";
            s.Guid = Guid.NewGuid();
            //s.Device = Guid.NewGuid();
            s.Description = $"Description{id}";
            s.Manufacturer = $"Manufacturer{id}";
            s.Model = $"Model{id}";
            s.DetailedSpecs = $"DetailedSpecs{id}";

            // configure other properties
            if (s.GetType() == typeof(Resources.CurrentSensor))
            {


            }
            else if (s.GetType() == typeof(Resources.TemperatureSensor))
            {

            }

            return s;
        }
    }

}