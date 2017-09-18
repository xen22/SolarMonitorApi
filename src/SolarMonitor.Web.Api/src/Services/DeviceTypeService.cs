using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SolarMonitor.Data.Repositories;

namespace SolarMonitorApi.Services
{
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly ILogger<DeviceTypeService> _logger;
        private IDeviceTypeRepository _repository;

        public DeviceTypeService(
            ILogger<DeviceTypeService> logger,
            IDeviceTypeRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<string> DeviceTypes()
        {
            return _repository.DeviceTypes();
        }
    }
}
