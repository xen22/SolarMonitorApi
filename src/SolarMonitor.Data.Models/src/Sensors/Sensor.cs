using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ct = SolarMonitor.Data.CommonTypes;

namespace SolarMonitor.Data.Models
{
    [Serializable]
    public class Sensor : ISensor, IDetailedDescriptor
    {
        public Sensor() { }
        public Sensor(Sensor s)
        {
            Id = s.Id;
            Type = s.Type;
            TypeId = s.TypeId;
            Name = s.Name;
            Guid = s.Guid;
            Device = s.Device;
            Manufacturer = s.Manufacturer;
            Model = s.Model;
            DetailedSpecs = s.DetailedSpecs;
            Description = s.Description;
        }
        public int Id { get; set; }

        [Required]
        public int TypeId { get; set; }
        public SensorType Type { get; set; }
        [Required]
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        public Guid? Device { get; set; }

        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string DetailedSpecs { get; set; }


        [Required]
        public int SiteId { get; set; }
        public Site Site { get; set; }

        [NotMapped]
        private string _siteName;
        [NotMapped]
        public virtual string SiteName { get { return _siteName; } set { _siteName = value; } }

        public static IEnumerable<ct.MeasurementType> SupportedMeasurements(ct.SensorType? sensorType)
        {
            if (sensorType == null)
            {
                return null;
            }

            List<Data.CommonTypes.MeasurementType> result = null;

            switch (sensorType)
            {
                case ct.SensorType.TemperatureSensor:
                    result = new List<Data.CommonTypes.MeasurementType>() {
                        ct.MeasurementType.MinTemperature,
                        ct.MeasurementType.MaxTemperature,
                        ct.MeasurementType.AvgTemperature,
                    };
                    break;
                case ct.SensorType.HumiditySensor:
                    result = new List<Data.CommonTypes.MeasurementType>() {
                        ct.MeasurementType.MinHumidity,
                        ct.MeasurementType.MaxHumidity,
                        ct.MeasurementType.AvgHumidity,
                    };
                    break;
                case ct.SensorType.Shunt:
                    result = new List<Data.CommonTypes.MeasurementType>() {
                        ct.MeasurementType.MinPower,
                        ct.MeasurementType.MaxPower,
                        ct.MeasurementType.AvgPower,
                        ct.MeasurementType.Energy,
                    };
                    break;
                default:
                    result = new List<Data.CommonTypes.MeasurementType>();
                    break;
            }
            // all types support MeasurementType.Default
            result.Add(ct.MeasurementType.Default);

            return result;
        }

        // [NotMapped]
        // public string SiteName { get { return Site.Name; } }
    }
}
