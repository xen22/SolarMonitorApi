using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SolarMonitor.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using SolarMonitor.Data.CommonTypes;
using SolarMonitorApi.Validators;
using SolarMonitorApi.RequestQueries;
using SolarMonitorApi.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;
using RestApiHelpers.Validation;
using Microsoft.AspNetCore.Authorization;

namespace SolarMonitorApi.Controllers
{
    /// Main controller class.
    [Route("api/[controller]")]
    [Authorize]
    public sealed class MeasurementsController : Controller
    {
        private readonly ILogger<MeasurementsController> _logger;
        private readonly IMeasurementsService _service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="service">MeasurementsService that performs all the work</param>
        public MeasurementsController(
            ILogger<MeasurementsController> logger,
            IMeasurementsService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET api/measurements
        /// <summary>
        /// Returns a list of measurements for a given sensor type or specific sensor
        /// </summary>
        /// <remarks>
        /// Each sensor can produce one or more measurement types, which can be specified here.
        /// For example, the TemperatureSensors can produce: InstantaneousTemperature, MinTemperature, MaxTemperature,  AvgTemperature.
        /// ShuntSensors can produce:  InstantaneousPower, MinPower, MaxPower, AvgPower, Energy.
        /// Some examples:
        /// * Get a list of (default) measurements from a specific sensor:
        ///   <code>/api/Measurements?SensorGuid=08519bc2-fbd5-11e6-8fa0-002522ab4073</code>
        /// * Get a list of (default) measurements from all temperature sensors:
        ///   <code>/api/Measurements?SensorType=TemperatureSensor</code>
        /// * Get the daily energy from a specific shunt sensor over the course of Feb 2017:
        ///   <code>/api/Measurements?SensorType=Shunt&amp;SensorGuid=08430889-fbd5-11e6-8fa0-002522ab4073&amp;Type=Energy&amp;Frequency=Daily&amp;Start=1/2/2017&amp;End=28/2/2017</code>
        /// </remarks>
        /// <param name="query"></param>
        /// <returns>A list of measurements that match the criteria</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "A list of measurements that match the criteria, the type depends on the specified sensor type. See example for a ShuntMeasurement.", Type = typeof(Resources.ShuntMeasurement))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "No measurements match the criteria.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "If the parameters are invalid")]
        [HttpGet]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        public IActionResult Get([FromQuery] GetMeasurementsRequestQuery query)
        {
            var measurements = _service.Get(query);
            if (measurements?.TotalCount == 0)
            {
                return NoContent();
            }
            return Ok(measurements);
        }
    }
}
