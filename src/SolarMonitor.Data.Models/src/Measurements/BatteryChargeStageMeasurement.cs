using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("BatteryChargeStageMeasurements")]
    public class BatteryChargeStageMeasurement : Measurement
    {
        public string ChargeStage { get; set; }
    }
}
