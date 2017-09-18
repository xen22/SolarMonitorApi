using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    //[Table("ChargeControllerMeasurements")]
    [NotMapped]
    public class to_delete_ChargeControllerMeasurement : Measurement
    {
        public float ArrayVoltage_V { get; set; }
        public float ArrayCurrent_A { get; set; }

        public float BatteryVoltage_V { get; set; }
        public float BatteryCurrent_A { get; set; }
        public float BatterySOC { get; set; }
        public string BatteryChargeStatus { get; set; }
        public float BatteryTemperature_C { get; set; }
        public float BatteryMinVoltage_V { get; set; }
        public float BatteryMaxVoltage_V { get; set; }

        public float ControllerTemperature_C { get; set; }

        public float LoadVoltage_V { get; set; }
        public float LoadCurrent_A { get; set; }
        public bool LoadOn { get; set; }

        public float EnergyConsumedDaily_kWh { get; set; }
        public float EnergyConsumedMonthly_kWh { get; set; }
        public float EnergyConsumedAnnual_kWh { get; set; }
        public float EnergyConsumedTotal_kWh { get; set; }
        public float EnergyGeneratedDaily_kWh { get; set; }
        public float EnergyGeneratedMonthly_kWh { get; set; }
        public float EnergyGeneratedAnnual_kWh { get; set; }
        public float EnergyGeneratedTotal_kWh { get; set; }
        public int Interval_s { get; set; }

        [Required]
        public int ChargeControllerId { get; set; }
        public ChargeController ChargeController { get; set; }
    }
}
