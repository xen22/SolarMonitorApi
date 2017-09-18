//using Lohmann.HALight;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace SolarMonitor.Data.Resources
{
    public class Site : Resource
    {
        public Site()
        : base(UriConstants.SitesPrefix)
        {
        }

        public Site(int id, string name, string timezone)
        : base(UriConstants.SitesPrefix)
        {
            Id = id;
            Name = name;
            Timezone = timezone;

            GenerateLinks();
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        [Required]
        public string Name { get; set; }
        public string Timezone { get; set; }

        public override void GenerateLinks()
        {
            if (Guid != Guid.Empty)
            {
                Links[Resources.UriConstants.LinkKeySelf] = new Resources.Link($"{Resources.UriConstants.SitesPrefix}/{Guid}");
            }
            if (!string.IsNullOrWhiteSpace(Name))
            {
                Links["devices"] = new Resources.Link($"{Resources.UriConstants.DevicesPrefix}?site={UrlEncoder.Default.Encode(Name.Trim())}");
                Links["sensors"] = new Resources.Link($"{Resources.UriConstants.SensorsPrefix}?site={UrlEncoder.Default.Encode(Name.Trim())}");
            }
        }

    }
}
