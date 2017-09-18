using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;

using SolarMonitor.Data;
using SolarMonitorApi.Validators;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http;
using SolarMonitorApi.RequestQueries;
using System.Collections.Generic;
using SolarMonitorApi.Helpers;
using SolarMonitorApi.Services;
using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using RestApiHelpers.Validation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace SolarMonitorApi.Controllers
{
    /// Devices controller class.
    [Route("api/[controller]")]
    [Authorize]
    public sealed class DevicesController : Controller
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly IDevicesService _service;
        private readonly IDeviceTypeService _deviceTypeService;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="service">The service that retrieves the data</param>
        /// <param name="deviceTypeService">The service required for DeviceTypes</param>
        public DevicesController(
            ILogger<DevicesController> logger,
            IDevicesService service,
            IDeviceTypeService deviceTypeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _deviceTypeService = deviceTypeService ?? throw new ArgumentNullException(nameof(deviceTypeService));
        }

        // GET api/devices/{guid}
        /// <summary> Returns a specific device by GUID </summary>
        /// <remarks>
        /// This API returns detailed information about the device, uniquely identified by its GUID.
        /// The returned type depends on the device type.
        /// </remarks>
        /// <param name="guid"></param>
        /// <returns>The device identified by GUID.</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "The device identified by GUID, depending on the device type. See example below for a ChargeController", Type = typeof(Resources.ChargeController))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "If the device does not exist")]
        [HttpGet("{guid}")]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        public IActionResult Get(Guid guid)
        {
            var device = _service.GetSingle(guid);
            if (device == null)
            {
                return NotFound();
            }
            return Ok(device);
        }

        // GET api/devices
        /// <summary>
        /// Returns devices based on query parameters
        /// </summary>
        /// <remarks>
        /// This API is useful when needing to list a set of devices in the system (along with generic information about them).
        /// Each device can then be queried individually for specific information related to its type through the /api/device/{guid} API.
        /// Examples:
        /// * Get a list of devices from a given site:
        ///   /api/Devices?Site=Simulation1
        /// * Get all ChargeController devices in the system:
        ///   /api/Devices?Type=ChargeController
        /// * Get a list of devices by name:
        ///   /api/Devices?Name=Battery1|Battery2|Battery3
        /// </remarks>
        /// <param name="query">The request query parameters</param>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns the devices", Type = typeof(Resources.CollectionResource<Resources.Device>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "No devices found matching the criteria")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid query parameters")]
        [HttpGet]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        public IActionResult Get([FromQuery] GetDevicesRequestQuery query)
        {
            var devices = _service.Get(query);
            if (devices?.TotalCount == 0)
            {
                return NoContent();
            }
            return Ok(devices);
        }

        // GET api/devices/types
        /// <summary>
        /// Returns the types of devices as an array of strings.
        /// </summary>
        /// <remarks>
        /// Note that the types of devices are returned as an array of strings (non-JSON, text/plain).
        /// The device type can be used when querying devices.
        /// See <seealso>/api/devices</seealso>
        /// </remarks>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns the device types", Type = typeof(List<string>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "No device types found in the system")]
        [HttpGet("Types")]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        public IActionResult GetTypes()
        {
            // Note: here we return the types in a plain Application/text format (as oposed to Application/HAL)
            // TODO: review this and see if we can use a simplified HAL format to make it consistent with the other APIs
            var deviceTypes = _deviceTypeService.DeviceTypes();
            if (deviceTypes?.Count() == 0)
            {
                return NoContent();
            }
            return Ok(deviceTypes);
        }

        // POST api/devices
        /// <summary>
        /// Creates a new device
        /// </summary>
        /// <remarks>
        /// This API is used to create a new device. 
        /// Example:
        /// 
        ///     POST /api/devices
        ///     {
        ///        "name": "Device1",
        ///        "typeId": 2,
        ///        "siteId": 1
        ///        // other parameters specific to the device type
        ///     }
        /// </remarks>
        /// <param name="device">The new device</param>
        /// <returns>The newly created resource</returns>
        [SwaggerResponse(StatusCodes.Status201Created, Description = "New device created successfully", Type = typeof(Resources.Site))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid query parameters")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, Description = "If the user does not have admin privileges")]
        [HttpPost]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        [Authorize(Roles = "admin")]
        public IActionResult Create([FromBody, Required] Resources.Device device)
        {
            var (uri, deviceResource) = _service.Create(device);
            return Created(uri, deviceResource);
        }


        // DELETE api/devices/{guid}
        /// <summary> Deletes a specific device by GUID </summary>
        /// <remarks>
        /// This API is used to delete a device.
        /// </remarks>
        /// <param name="guid">The device GUID</param>
        /// <returns>The deleted device.</returns>
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "The device has been deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid GUID")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, Description = "If the user does not have admin privileges")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "If the device does not exist")]
        [HttpDelete("{guid}")]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        [Authorize(Roles = "admin")]
        public IActionResult delete(Guid guid)
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
