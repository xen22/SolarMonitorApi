//using Lohmann.HALight;

using System;
using System.ComponentModel.DataAnnotations;

namespace SolarMonitor.Data.Resources
{
    /// <summary>
    /// This class is for documentation purposes only via Swagger (see SensorsController.Create() action)
    /// The main reason we need it is because Swagger does not have an easy way of hiding properties that are
    /// not required for Post actions.
    /// </summary>
    public class SensorForPost
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Name { get; set; }
        public string Site { get; set; }


        public string Description { get; set; }

        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string DetailedSpecs { get; set; }
    }
}