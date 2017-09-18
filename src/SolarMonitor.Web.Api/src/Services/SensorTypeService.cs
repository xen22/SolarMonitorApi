using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SolarMonitor.Data.Repositories;

namespace SolarMonitorApi.Services
{
    public class SensorTypeService : ISensorTypeService
    {
        private readonly ILogger<SensorTypeService> _logger;
        private ISensorTypeRepository _repository;

        public SensorTypeService(
            ILogger<SensorTypeService> logger,
            ISensorTypeRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<string> SensorTypes()
        {
            return _repository.SensorTypes();
        }
    }
}
