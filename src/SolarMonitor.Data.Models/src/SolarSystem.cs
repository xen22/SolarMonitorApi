using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

// namespace SolarMonitor.Data.Models
// {
//     [Table("SolarSystems")]
//     public class SolarSystem : IDescriptor
//     {
//         public SolarSystem()
//         {
//             SolarArrays = new List<SolarArray>();
//             ChargeControllers = new List<ChargeController>();
//             BatteryBanks = new List<BatteryBank>();
//             Inverters = new List<Inverter>();
//             Shunts = new List<Shunt>();
//             CurrentSensors = new List<CurrentSensor>();
//         }

//         public int Id { get; set; }

//         public string Name { get; set; }
//         public string Description { get; set; }

//         public ICollection<SolarArray> SolarArrays { get; set; }
//         public ICollection<ChargeController> ChargeControllers { get; set; }
//         public ICollection<BatteryBank> BatteryBanks { get; set; }
//         public ICollection<Inverter> Inverters { get; set; }
//         public ICollection<Shunt> Shunts { get; set; }
//         public ICollection<CurrentSensor> CurrentSensors { get; set; }

//         [Required]
//         public int SiteId { get; set; }
//         public Site Site { get; set; }
//     }
// }
