using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace SolarMonitor.Data.Models
{
    [Table("Sites")]
    [Serializable]
    public class Site : IEntity, IEquatable<Site>
    {
        public Site()
        {
            // SolarSystems = new List<SolarSystem>();
            // WeatherBases = new List<WeatherBase>();
        }

        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        [Required, MaxLength(45)]
        public string Name { get; set; }
        public string Timezone { get; set; }

        public bool Equals(Site other)
        {
            return (other != null &&
              Id == other.Id &&
              Name == other.Name &&
              Timezone == other.Timezone);
        }

        // [Required]
        // public virtual ICollection<SolarSystem> SolarSystems { get; set; } 
        // public virtual ICollection<WeatherBase> WeatherBases { get; set; } 
    }
}
