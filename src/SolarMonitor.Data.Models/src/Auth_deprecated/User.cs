using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("Users")]
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }
        public string Password { get; set; }

        public ICollection<RoleAssignment> RoleAssignments { get; set; }
    }
}
