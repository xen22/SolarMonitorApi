using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("ChargeControllers")]
    public class ChargeController : Device
    {
        public float? CurrentRating_A { get; set; }

        // [Required]
        // public int SolarSystemId {get; set;}
        // public SolarSystem SolarSystem { get; set; }

        //    public BatteryBank ConnectedBatteryBank { get; set; }
    }
}
