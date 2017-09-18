using SolarMonitor.Data.Repositories;
using System.Linq;

namespace SolarMonitorApi.Validators
{
    class SensorTypeValidator : ISensorTypeValidator
    {

        ISensorTypeRepository _repo;
        public SensorTypeValidator(ISensorTypeRepository repo)
        {
            _repo = repo;
        }

        public void Validate(string type)
        {
            if ((type == null) || (_repo.SensorTypes().Select(t => t == type).Count() == 0))
            {
                throw new InvalidParameterException($"SensorType {type} not found in the database");
            }
        }
    }
}
