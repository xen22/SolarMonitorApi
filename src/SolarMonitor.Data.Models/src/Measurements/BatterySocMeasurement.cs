using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("BatterySocMeasurements")]
    public class BatterySocMeasurement : Measurement
    {
        public int SOC { get; set; }
    }
}
