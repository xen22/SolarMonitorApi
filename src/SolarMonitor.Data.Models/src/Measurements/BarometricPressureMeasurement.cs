using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("BarometricPressureMeasurements")]
    public class BarometricPressureMeasurement : Measurement
    {
        public float BarometricPressure_mBar { get; set; }
    }
}
