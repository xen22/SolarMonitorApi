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
using Newtonsoft.Json;
using SolarMonitor.Data.Adapters;

namespace SolarMonitorApi.Services
{
    class SensorsService : ISensorsService
    {
        private readonly ILogger<SensorsService> _logger;
        private readonly ISensorRepository _repository;
        private readonly ISensorAdapter _sensorAdapter;

        public SensorsService(
            ILogger<SensorsService> logger,
            ISensorRepository repository,
            ISensorAdapter sensorAdapter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _sensorAdapter = sensorAdapter ?? throw new ArgumentNullException(nameof(sensorAdapter));
        }

        public Resources.CollectionResource<Resources.Sensor> Get(GetSensorsRequestQuery query)
        {
            _logger.LogDebug("Getting sensors based on query.");

            var sensors = _repository.FindSensors(
                pageIndex: query.PageIndex ?? QueryParameterDefaults.PageIndex,
                pageSize: query.PageSize ?? QueryParameterDefaults.PageSize,
                sensorTypes: ParamSplitter.SplitParam(query.Type),
                sensorNames: ParamSplitter.SplitParam(query.Name),
                siteNames: ParamSplitter.SplitParam(query.Site)
            );

            //var sensorResources = sensors.Select(s => _genericSensorAdapter.ModelToResource(s));
            var sensorResources = sensors.Select(s => _sensorAdapter.ModelToResource(s));
            var collection = new Resources.CollectionResource<Resources.Sensor>(Resources.UriConstants.SensorsPrefix,
                sensors.PageIndex, sensors.PageSize, sensors.TotalCount, sensorResources.ToList());

            return collection;
        }

        public Resources.Sensor GetSingle(Guid guid)
        {
            _logger.LogDebug($"Getting single sensor with GUID {guid}");

            var sensor = _repository.Sensor(guid);
            if (sensor == null)
            {
                return null;
            }
            return _sensorAdapter.ModelToResource(sensor);
        }

        public (string uri, Resources.Sensor) Create(Resources.Sensor sensor)
        {
            try
            {
                var sensorModel = _sensorAdapter.ResourceToModel(sensor);
                var savedSensorModel = _repository.Create(sensorModel);
                var savedsensorResource = _sensorAdapter.ModelToResource(savedSensorModel);

                return (savedsensorResource.Links[Resources.UriConstants.LinkKeySelf].HRef, savedsensorResource);
            }
            catch (Exception)
            {
                // TODO: need a better way of determining what went wrong with the conversions above
                // and return error message to the user in the response (along with BadRequest status)
                return (null, null);
            }
        }

        public bool Delete(Guid guid)
        {
            return _repository.Delete(guid);
        }
    }
}