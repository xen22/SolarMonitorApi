using System;

namespace SolarMonitor.Data.Resources
{
    public class Measurement : Resource
    {
        public Measurement()
        : base(UriConstants.MeasurementsPrefix)
        { }
        public string Name { get; set; }
        public string Type { get; set; }
        public System.DateTime Timestamp { get; set; }

        public Guid SensorGuid { get; set; }

        public override void GenerateLinks()
        {
            // we don't include links with measurement resources 
        }
    }
}
