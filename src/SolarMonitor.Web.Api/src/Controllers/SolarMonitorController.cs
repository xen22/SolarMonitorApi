using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.AspNetCore.Cors;
using SolarMonitor.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
using SolarMonitorApi.Validators;
using SolarMonitorApi.Helpers;
using SolarMonitorApi.Services;
using SolarMonitorApi.Exceptions;
using RestApiHelpers.Validation;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace SolarMonitorApi.Controllers
{
    /// SolarMonitor controller class.
    [Route("api/[controller]")]
    [Authorize]
    public sealed class SolarMonitorController : Controller
    {
        private readonly ILogger<SolarMonitorController> _logger;
        private readonly IConfiguration _config;
        private readonly string _version;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="config">Configuration object</param>
        public SolarMonitorController(ILogger<SolarMonitorController> logger,
                                      IConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _version = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        // GET api/version
        /// <summary>Returns service version </summary> 
        /// <remarks>The version is returned in plain text.</remarks>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Logout suceeded", Type = typeof(string))]
        [HttpGet("version")]
        public async Task<IActionResult> GetVersionAsync()
        {
            try
            {
                var delay = int.Parse(_config["SimulatedDelay"]);
                await Task.Delay(delay);
            }
            catch { }
            return Ok(_version);
        }

        [Authorize]
        [HttpGet("claims")]
        public object Claims()
        {
            return User.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }
    }
}
