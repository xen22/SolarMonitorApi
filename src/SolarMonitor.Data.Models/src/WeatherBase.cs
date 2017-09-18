using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SolarMonitor.Data.Models
{
    [Table("WeatherBases")]
    public class WeatherBase : IDescriptor
    {
        public WeatherBase()
        {
            WeatherStations = new List<WeatherStation>();
            TemperatureSensors = new List<TemperatureSensor>();
            HumiditySensors = new List<HumiditySensor>();
            BarometricPressureSensors = new List<BarometricPressureSensor>();
            WindSensors = new List<WindSensor>();
        }
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<WeatherStation> WeatherStations { get; set; }
        public virtual ICollection<TemperatureSensor> TemperatureSensors { get; set; }
        public virtual ICollection<HumiditySensor> HumiditySensors { get; set; }
        public virtual ICollection<BarometricPressureSensor> BarometricPressureSensors { get; set; }
        public virtual ICollection<WindSensor> WindSensors { get; set; }

        [Required]
        public int SiteId { get; set; }
        public Site Site { get; set; }
    }
}
