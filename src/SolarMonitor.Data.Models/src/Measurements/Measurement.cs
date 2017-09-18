using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    public class Measurement : IMeasurement
    {
        public int Id { get; set; }
        public System.DateTime Timestamp { get; set; }

        [Required]
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}
