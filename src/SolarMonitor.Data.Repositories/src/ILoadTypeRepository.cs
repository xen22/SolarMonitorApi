using SolarMonitor.Data.Models;

namespace SolarMonitor.Data.Repositories
{
    public interface ILoadTypeRepository
    {
        LoadType GetByType(string type);
    }
}