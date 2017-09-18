using System;
using System.Collections.Generic;
using SolarMonitor.Data.Repositories;
using System.Linq;
using Microsoft.Extensions.Logging;
using SolarMonitorApi.RequestQueries;
using CommonTypes = SolarMonitor.Data.CommonTypes;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitorApi.Services
{
    class MeasurementsService : IMeasurementsService
    {
        private ILogger<MeasurementsService> _logger;
        private IMeasurementRepository _repository;


        public MeasurementsService(
            ILogger<MeasurementsService> logger,
            IMeasurementRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Resources.CollectionResource<Resources.Measurement> Get(GetMeasurementsRequestQuery query)
        {
            _logger.LogDebug("Getting measurements.");

            IEnumerable<Models.IMeasurement> measurements = _repository.FindMeasurements(
                pageIndex: query.PageIndex ?? QueryParameterDefaults.PageIndex,
                pageSize: query.PageSize ?? QueryParameterDefaults.PageSize
            );

            // SensorGuid takes precedence over SensorType
            if (query.SensorGuid != null)
            {
                measurements = measurements.Where(m => m.Sensor.Guid == query.SensorGuid);
            }
            else if ((query.SensorType != null) &&
                     (query.SensorType != CommonTypes.SensorType.Unset))
            {
                measurements = measurements.Where(m => m.Sensor.Type.Name == query.SensorType.ToString());
            }

            // MeasurementType: Min, Max, Average, Energy

            // TODO:
            // 4. obtain filtering expression based on MeasurementType and MeasurementFrequency
            // 5. apply filtering expression to _repository.measurements() function
            // 6. apply start and end filters if non-null (also decide what to do if one or both of them are null, i.e. default behaviour)

            int totalCount = measurements.Count();
            int page = query.PageIndex ?? QueryParameterDefaults.PageIndex;
            int count = query.PageSize ?? QueryParameterDefaults.PageSize;

            measurements = measurements
                .Skip((page - 1) * count)
                .Take(count);

            // var adapter = _adapterFactory.CreateAdapter(query.SensorType.ToString());
            // if (adapter == null)
            // {
            //     throw new NullReferenceException("Cannot get the measurement adapter.");
            // }
            // var measurementResources = measurements.Select(s => adapter.ModelToResource(s));
            // var collection = new Resources.CollectionResource<Resources.Measurement>(Resources.UriConstants.MeasurementsPrefix, page, count, totalCount, measurementResources.ToList());

            // return collection;

            return null;
        }

    }
}




// -----------------------------------------------
// old code
/*
            var repo = _repoFactory.repository(query.SensorType.ToString());
            if (repo == null)
            {
                throw new NullReferenceException($"Cannot get IMeasurementRepository for type {query.Type}");
            }

            var adapter = _adapterFactory.adapter(query.SensorType.ToString());
            if (adapter == null)
            {
                throw new NullReferenceException($"Cannot get IMeasurementAdapter for type {query.Type}");
            }

            // 2. Get measurements from repository based on sensor type
            var measurements = repo.measurements();

            // By default (if neither Start nor End are specified), we look for measurements going backwards
            // from the current time
            var direction = MeasurementSelectionDirection.Backward;

            // 3. Build a LINQ Query
            //   3.1 For certain measurement types (e.g. Energy), run a SQL stored procedure
            //   3.2 For all other measurement types, build a LINQ query and run it

            // Apply optional filters
            if (query.SensorGuid != null)
            {
                measurements = measurements.Where(m => m.Sensor.Guid == query.SensorGuid);
            }

            // If a Start time is specified, we need to look for measurements from then
            // (Start takes precedence over End)
            if (query.Start != null)
            {
                measurements = measurements.Where(m => m.Timestamp >= query.Start);
                direction = MeasurementSelectionDirection.Forward;
            }
            if (query.End != null)
            {
                measurements = measurements.Where(m => m.Timestamp <= query.End);
            }

            if (direction == MeasurementSelectionDirection.Forward)
            {

            }
            else if (direction == MeasurementSelectionDirection.Backward)
            {

            }

            if (query.Type == MeasurementType.Energy)
            {
                // this is a special case: run a SQL stored procedure

            }
            else
            {
                // build a LINQ query


                // var results = measurements
                //     //.Select(m => m.Timestamp)
                //     .Select(m => new {
                //         Temperature = ((SolarMonitor.Data.Models.TemperatureMeasurement)m).Temperature_C
                //     })
                //     .GroupBy(
                //         m => m.Timestamp, 
                //         m => m.Timestamp.ToString(""), 
                //         (ts, vals) => new { Timestamp = ts, Measurements = vals.ToList()});

                //measurements = measurements
                //.Where(m => cmd.Name == "" ? true : ParamSplitter.splitParam(cmd.Name).Contains(m.SensorName));


            }

            // 4. Apply other parameters (e.g. page count)
            // 5. Convert measurement model instances to resources

            return new OkObjectResult(null);
*/




/*



            measurements = measurements
                .Where(m => cmd.Name == "" ? true : ParamSplitter.splitParam(cmd.Name).Contains(m.SensorName));

            var totalCount = measurements.Count();

            measurements = measurements
                .Skip((cmd.Page - 1) * cmd.Count)
                .Take(cmd.Count);

            var measurementResources = measurements.Select(m => adapter.ModelToResource(m));
            var collection = new Resources.CollectionResource<Resources.Measurement>(
                Resources.Constants.MeasurementsPrefix, cmd.Page, cmd.Count, totalCount, measurementResources.ToList());

            return new OkObjectResult(collection);
*/

// // TODO: apply filters to this query based on Request.Query parameters
// var selectedMeasurements = repo.measurements();
// if(cmd.Name != "") {
//   selectedMeasurements = selectedMeasurements.Where(m => m.SensorName == cmd.Name);
// }

// var measurementResources = new List<Resources.Measurement>();

// foreach(var m in selectedMeasurements)
// {
//   measurementResources.Add(adapter.ModelToResource(m));
// }
// return new OkObjectResult(measurementResources);