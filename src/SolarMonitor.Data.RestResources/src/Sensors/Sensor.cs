//using Lohmann.HALight;

using System;
using System.ComponentModel.DataAnnotations;

namespace SolarMonitor.Data.Resources
{
    public class Sensor : Resource
    {
        public Sensor()
        : base(UriConstants.SensorsPrefix)
        { }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Name { get; set; }
        public Guid Guid { get; set; }

        [Required]
        public string Site { get; set; }


        public string Description { get; set; }

        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string DetailedSpecs { get; set; }

        public override void GenerateLinks()
        {
            Links[Resources.UriConstants.LinkKeySelf] = new Resources.Link($"{Resources.UriConstants.SensorsPrefix}/{Guid}");
        }
    }

}