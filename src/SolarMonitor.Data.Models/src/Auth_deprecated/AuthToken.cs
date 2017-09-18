using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("AuthTokens")]
    public class AuthToken
    {
        [Key]
        public Guid Guid { get; set; }
        [Required]
        public DateTime Expiry { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
