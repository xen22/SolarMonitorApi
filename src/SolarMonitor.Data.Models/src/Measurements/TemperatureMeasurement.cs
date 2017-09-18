using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("TemperatureMeasurements")]
    public class TemperatureMeasurement : Measurement
    {
        public float Temperature_C { get; set; }
    }
}
