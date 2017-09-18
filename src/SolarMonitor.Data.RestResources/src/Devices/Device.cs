//using Lohmann.HALight;

using System;

namespace SolarMonitor.Data.Resources
{
    public class Device : Resource
    {

        public Device()
        : base(UriConstants.DevicesPrefix)
        { }

        public string Name { get; set; }
        public Guid Guid { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string DetailedSpecs { get; set; }

        public string SiteName { get; set; }

        public void FromBaseDevice(Device d)
        {
            Links = d.Links;

            Name = d.Name;
            Guid = d.Guid;
            Type = d.Type;
            Description = d.Description;
            Manufacturer = d.Manufacturer;
            Model = d.Model;
            DetailedSpecs = d.DetailedSpecs;
            SiteName = d.SiteName;
        }

        public override void GenerateLinks()
        {
            Links[Resources.UriConstants.LinkKeySelf] = new Resources.Link($"{Resources.UriConstants.DevicesPrefix}/{Guid}");
        }
    }

}
