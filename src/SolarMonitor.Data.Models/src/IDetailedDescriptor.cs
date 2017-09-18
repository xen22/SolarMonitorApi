namespace SolarMonitor.Data.Models
{
    public interface IDetailedDescriptor : IDescriptor
    {
        string Manufacturer { get; set; }
        string Model { get; set; }
        string DetailedSpecs { get; set; }
    }
}
