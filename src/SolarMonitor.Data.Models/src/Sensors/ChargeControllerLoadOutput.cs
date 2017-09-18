using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("ChargeControllerLoadOutputs")]
    public class ChargeControllerLoadOutput : Sensor
    {
    }
}
