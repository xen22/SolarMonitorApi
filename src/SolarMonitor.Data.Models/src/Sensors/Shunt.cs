using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SolarMonitor.Data.Models
{
    [Table("Shunts")]
    public class Shunt : Sensor
    {
        public float? InternalResistor_mOhm { get; set; }
        public float? MaxCurrent_A { get; set; }
        public float? InternalVoltage_mV { get; set; }
    }
}
