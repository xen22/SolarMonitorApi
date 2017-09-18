using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("ChargeControllerLoadOutputMeasurements")]
    public class ChargeControllerLoadOutputMeasurement : Measurement
    {
        public bool On { get; set; }
    }
}
