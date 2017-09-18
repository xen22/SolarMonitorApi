//using Lohmann.HALight;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace SolarMonitor.Data.Resources
{
    /// <summary>
    /// This class is for documentation purposes only via Swagger (see SitesController.Create() action)
    /// The main reason we need it is because Swagger does not have an easy way of hiding properties that are
    /// not required for Post actions.
    /// </summary>
    public class SiteForPost
    {
        [Required]
        public string Name { get; set; }
        public string Timezone { get; set; }
    }
}
