using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("LoadTypes")]
    public class LoadType : IEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public float Voltage_V { get; set; }
    }
}
