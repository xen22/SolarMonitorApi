using System;

namespace SolarMonitor.Data.Models
{
    public interface IMeasurement : IEntity
    {
        System.DateTime Timestamp { get; set; }
        Sensor Sensor { get; }
    }
}
