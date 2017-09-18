using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("BarometricPressureSensors")]
    public class BarometricPressureSensor : Sensor
    {
    }
}
