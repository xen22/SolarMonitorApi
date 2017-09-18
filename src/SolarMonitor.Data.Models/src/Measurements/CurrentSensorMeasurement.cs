using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("CurrentSensorMeasurements")]
    public class CurrentSensorMeasurement : Measurement
    {
        public float Current_A { get; set; }
        public int Interval_s { get; set; }
    }
}
