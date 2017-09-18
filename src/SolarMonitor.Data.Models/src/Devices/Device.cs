using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    public class Device : IDevice, IDetailedDescriptor
    {
        public Device() { }
        public Device(Device d)
        {
            Id = d.Id;
            Type = d.Type;
            Name = d.Name;
            Guid = d.Guid;
        }

        // IDevice
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TypeId { get; set; }
        public DeviceType Type { get; set; }

        [Required]
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        // IDetailedDescriptor
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
    }
}
