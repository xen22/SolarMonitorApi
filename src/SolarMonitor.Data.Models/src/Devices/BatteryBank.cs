using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SolarMonitor.Data.Models
{
    [Table("BatteryBanks")]
    public class BatteryBank : Device
    {

        public string Configuration { get; set; }
        public int? NumBatteries { get; set; }

        public float? CapacityPerBattery_Ah { get; set; }
        public float? TotalCapacity_Ah { get; set; }
        public float? BankVoltage_V { get; set; }
        public float? BatteryVoltage_V { get; set; }

        // [Required]
        // public int SolarSystemId {get; set;}
        // public SolarSystem SolarSystem { get; set; }

        // public SolarController ConnectedSolarController { get; set; }
        // public Inverter ConnectedInverter { get; set; }
    }
}
