using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("Inverters")]
    public class Inverter : Device
    {
        public float? MaxContinuousPower_W { get; set; }
        public float? MaxSurgePower_W { get; set; }
        public float? InputVoltage_V { get; set; }
        public float? OutputVoltage_V { get; set; }

        // [Required]
        // public int SolarSystemId {get; set;}
        // public SolarSystem SolarSystem { get; set; }

        //    public BatteryBank ConnectedBatteryBank { get; set; }
    }
}
