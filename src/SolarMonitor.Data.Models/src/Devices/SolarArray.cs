using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("SolarArrays")]
    public class SolarArray : Device
    {

        public string Configuration { get; set; }
        public int? NumPanels { get; set; }

        public int? PanelMaxPower_W { get; set; }
        public float? PanelOpenCircuitVoltage_V { get; set; }
        public float? PanelShortCircuitCurrent_A { get; set; }

        // [Required]
        // public int SolarSystemId {get; set;}
        // public SolarSystem SolarSystem { get; set; }

        //    public SolarController ConnectedController { get; set; }
    }
}
