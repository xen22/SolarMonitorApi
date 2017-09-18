using System;

namespace SolarMonitor.Data.Models
{
    public interface ISite : IEntity
    {
        string Name { get; set; }
    }
}
