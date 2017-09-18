using System;

namespace SolarMonitor.Data.Models
{
    public interface ISensor : IEntity
    {
        SensorType Type { get; set; }
        string Name { get; set; }
        Guid Guid { get; set; }
        Guid? Device { get; set; }
        Site Site { get; set; }
    }
}
