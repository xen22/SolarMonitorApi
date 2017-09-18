using System;

namespace SolarMonitor.Data.Models
{
    public interface IDevice : IEntity
    {
        int TypeId { get; set; }
        DeviceType Type { get; set; }
        string Name { get; set; }
        Guid Guid { get; set; }

        Site Site { get; set; }
    }
}
