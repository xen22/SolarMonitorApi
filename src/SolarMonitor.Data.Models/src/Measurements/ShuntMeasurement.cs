using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("ShuntMeasurements")]
    public class ShuntMeasurement : Measurement
    {
        public float Current_A { get; set; }
        public float Voltage_v { get; set; }
    }
}
