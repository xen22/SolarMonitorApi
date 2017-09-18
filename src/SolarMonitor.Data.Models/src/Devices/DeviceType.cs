using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("DeviceTypes")]
    public class DeviceType : IEntity
    {

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
