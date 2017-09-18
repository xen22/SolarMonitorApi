using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("EnergyStatsMeasurements")]
    public class EnergyStatsMeasurement : Measurement
    {
        public float DailyEnergy_kWh { get; set; }
        public float MonthlyEnergy_kWh { get; set; }
        public float AnnualEnergy_kWh { get; set; }
        public float TotalEnergy_kWh { get; set; }
    }
}
