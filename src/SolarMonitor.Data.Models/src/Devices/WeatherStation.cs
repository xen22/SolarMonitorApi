using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("WeatherStations")]
    public class WeatherStation : Device
    {

        // [Required]
        // public int WeatherBaseId {get; set;}
        // public WeatherBase WeatherBase { get; set; }
    }
}
