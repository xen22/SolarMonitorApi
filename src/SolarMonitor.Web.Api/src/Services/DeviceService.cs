using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SolarMonitor.Data.Repositories;
using SolarMonitorApi.Helpers;
using SolarMonitorApi.RequestQueries;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;
using CommonTypes = SolarMonitor.Data.CommonTypes;
using SolarMonitor.Data.Adapters;

namespace SolarMonitorApi.Services
{
    public class DeviceService : IDevicesService
    {
        private readonly ILogger<DeviceService> _logger;
        private readonly IDeviceRepository _repository;
        private readonly IDeviceAdapter _deviceAdapter;


        public DeviceService(
            ILogger<DeviceService> logger,
            IDeviceRepository repository,
            IDeviceAdapter deviceAdapter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _deviceAdapter = deviceAdapter ?? throw new ArgumentNullException(nameof(deviceAdapter));
        }

        public Resources.CollectionResource<Resources.Device> Get(GetDevicesRequestQuery query)
        {
            _logger.LogDebug("Getting Devices based on query.");

            var devices = _repository.FindDevices(
                pageIndex: query.PageIndex ?? QueryParameterDefaults.PageIndex,
                pageSize: query.PageSize ?? QueryParameterDefaults.PageSize,
                deviceTypes: ParamSplitter.SplitParam(query.Type.ToString()),
                deviceNames: ParamSplitter.SplitParam(query.Name),
                siteNames: ParamSplitter.SplitParam(query.Site)
            );

            var sensorResources = devices.Select(d => _deviceAdapter.ModelToResource(d));
            var collection = new Resources.CollectionResource<Resources.Device>(Resources.UriConstants.DevicesPrefix,
                devices.PageIndex, devices.PageSize, devices.TotalCount, sensorResources.ToList());

            return collection;
        }

        public Resources.Device GetSingle(Guid guid)
        {
            _logger.LogDebug($"Getting single Device with GUID {guid}");
            var device = _repository.Device(guid);
            if (device == null)
            {
                return null;
            }
            return _deviceAdapter.ModelToResource(device);
        }

        public (string uri, Resources.Device) Create(Resources.Device device)
        {
            var deviceModel = _deviceAdapter.ResourceToModel(device);
            var savedDeviceModel = _repository.Create(deviceModel);
            var savedDeviceRes = _deviceAdapter.ModelToResource(savedDeviceModel);

            return (savedDeviceRes.Links[Resources.UriConstants.LinkKeySelf].HRef, savedDeviceRes);
        }

        public bool Delete(Guid guid)
        {
            return _repository.Delete(guid);
        }
    }
}
