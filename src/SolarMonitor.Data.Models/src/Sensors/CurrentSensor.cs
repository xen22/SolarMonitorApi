using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SolarMonitor.Data.Models
{
    [Table("CurrentSensors")]
    public class CurrentSensor : Sensor
    {
        public LoadType LoadType { get; set; }
    }
}
