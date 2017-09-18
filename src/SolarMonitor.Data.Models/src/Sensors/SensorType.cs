using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("SensorTypes")]
    [Serializable]
    public class SensorType : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
