using SolarMonitor.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using SolarMonitorApi.Validators;
using SolarMonitorApi.RequestQueries;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using SolarMonitorApi.Helpers;
using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using SolarMonitorApi.Services;
using RestApiHelpers.Validation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace SolarMonitorApi.Controllers
{
    /// Sensors controller class.
    [Route("api/[controller]")]
    [Authorize]
    public sealed class SensorsController : Controller
    {
        private readonly ILogger<SensorsController> _logger;
        private readonly ISensorsService _service;
        private readonly ISensorTypeService _sensorTypeService;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="service">The service that retrieves the data</param>
        /// <param name="sensorTypeService">The service required for SensorTypes</param>
        public SensorsController(
            ILogger<SensorsController> logger,
            ISensorsService service,
            ISensorTypeService sensorTypeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _sensorTypeService = sensorTypeService ?? throw new ArgumentNullException(nameof(sensorTypeService));
        }

        // GET api/sensors/{guid}
        /// <summary> Returns a specific sensor by GUID </summary>
        /// <remarks>
        /// This API returns detailed information about the sensor, uniquely identified by its GUID.
        /// The returned type depends on the sensor type.
        /// </remarks>
        /// <param name="guid"></param>
        /// <returns>The Sensor identified by GUID.</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "The sensor identified by GUID, depending on the sensor type. See example below for a ShuntSensor", Type = typeof(Resources.Shunt))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "If the sensor does not exist")]
        [HttpGet("{guid}")]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        public IActionResult Get(Guid guid)
        {
            var sensor = _service.GetSingle(guid);
            if (sensor == null)
            {
                return NotFound();
            }
            return Ok(sensor);
        }

        // GET api/sensors
        /// <summary>
        /// Returns sensors based on query parameters
        /// </summary>
        /// <remarks>
        /// This API is useful when needing to list a set of sensors in the system (along with generic information about them).
        /// Each sensor can then be queried individually for specific information related to its type through the /Api/Sensor/{Guid} API.
        /// Examples:
        /// * Get a list of sensors from a given site:
        ///   /api/Sensors?Site=Simulation1
        /// * Get all temperature sensors in the system:
        ///   /api/Sensors?Type=TemperatureSensor
        /// * Get a list of sensors by name:
        ///   /api/Sensors?Name=ShuntA|ShuntB|ShuntC
        /// </remarks>
        /// <param name="query">The request query parameters</param>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns the sensors", Type = typeof(Resources.CollectionResource<Resources.Sensor>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "No sensors found matching the criteria")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid query parameters")]
        [HttpGet]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        public IActionResult Get([FromQuery]GetSensorsRequestQuery query)
        {
            var sensors = _service.Get(query);
            if (sensors?.TotalCount == 0)
            {
                return NoContent();
            }
            return Ok(sensors);
        }

        // GET api/sensors/types
        /// <summary>
        /// Returns the types of sensors as an array of strings.
        /// </summary>
        /// <remarks>
        /// Note that the types of sensors are returned as an array of strings (non-JSON, text/plain).
        /// The sensor type can be used when querying sensors and measurements.
        /// See <seealso>/api/sensors</seealso> and <seealso>/api/measurements</seealso>
        /// </remarks>
        /// <returns>A list of suported sensor types</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns the sensor types", Type = typeof(List<string>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "No sensor types found in the system")]
        [HttpGet("Types")]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        public IActionResult GetTypes()
        {
            // Note: here we return the types in a plain Application/text format (as oposed to Application/HAL)
            // TODO: review this and see if we can use a simplified HAL format to make it consistent with the other APIs
            var sensorTypes = _sensorTypeService.SensorTypes();
            if (sensorTypes?.Count() == 0)
            {
                return NoContent();
            }
            return Ok(sensorTypes);
        }

        // POST api/sensors
        /// <summary>
        /// Creates a new sensor
        /// </summary>
        /// <remarks>
        /// This API is used to create a new sensor. 
        /// Example:
        /// 
        ///     POST /api/sensors
        ///     {
        ///        "name": "sensor1",
        ///        "type": "Shunt",
        ///        "site": "Simulation 1"
        ///        // other parameters specific to the sensor type
        ///     }
        /// </remarks>
        /// <param name="sensor">The new sensor</param>
        /// <returns>The newly created resource</returns>
        [SwaggerResponse(StatusCodes.Status201Created, Description = "New sensor created successfully", Type = typeof(Resources.Sensor))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid query parameters")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, Description = "If the user does not have admin privileges")]
        [HttpPost]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        [Authorize(Roles = "admin")]
        public IActionResult Create([FromBody, Required] Resources.Sensor sensor)
        {
            var (uri, sensorResource) = _service.Create(sensor);
            if (sensorResource == null)
            {
                return BadRequest();
            }
            return Created(uri, sensorResource);
        }

        // DELETE api/sensors/{guid}
        /// <summary> Deletes a specific sensor by GUID </summary>
        /// <remarks>
        /// This API is used to delete a sensor.
        /// </remarks>
        /// <param name="guid">The sensor GUID</param>
        /// <returns>The deleted sensor.</returns>
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "The sensor has been deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid GUID")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, Description = "If the user does not have admin privileges")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "If the sensor does not exist")]
        [HttpDelete("{guid}")]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(Guid guid)
        {
            bool result = _service.Delete(guid);
            if (result == false)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
