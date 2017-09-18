using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarMonitor.Data.Models
{
    [Table("Roles")]
    public class Role : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoleAssignment> RoleAssignments { get; set; }
    }

}