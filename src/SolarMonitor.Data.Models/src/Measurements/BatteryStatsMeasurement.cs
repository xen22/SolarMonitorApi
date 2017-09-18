using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("BatteryStatsMeasurements")]
    public class BatteryStatsMeasurement : Measurement
    {
        public float MinVoltage { get; set; }
        public float MaxVoltage { get; set; }
    }
}
