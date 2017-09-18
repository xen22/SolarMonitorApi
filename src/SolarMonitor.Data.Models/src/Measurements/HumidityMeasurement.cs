using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("HumidityMeasurements")]
    public class HumidityMeasurement : Measurement
    {
        public float RelativeHumidity { get; set; }
    }
}
