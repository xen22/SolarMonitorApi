using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("TemperatureSensors")]
    [Serializable]
    public class TemperatureSensor : Sensor, IDetailedDescriptor
    {
    }
}
