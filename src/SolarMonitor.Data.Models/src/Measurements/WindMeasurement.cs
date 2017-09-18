using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("WindMeasurements")]
    public class WindMeasurement : Measurement
    {
        public float WindSpeed_mps { get; set; }
        public float WindDirection_degFromN { get; set; }
    }
}
