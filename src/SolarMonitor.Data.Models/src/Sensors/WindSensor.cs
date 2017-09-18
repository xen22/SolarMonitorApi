using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("WindSensors")]
    public class WindSensor : Sensor, IDetailedDescriptor
    {
    }
}
