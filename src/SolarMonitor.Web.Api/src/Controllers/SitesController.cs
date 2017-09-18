using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

using SolarMonitor.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Http;
using SolarMonitorApi.RequestQueries;
using SolarMonitorApi.Services;
using SolarMonitorApi.Validators;
using System;
using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using System.ComponentModel.DataAnnotations;
using RestApiHelpers.Validation;
using Microsoft.AspNetCore.Authorization;

namespace SolarMonitorApi.Controllers
{
    /// Sites controller class.
    [Route("api/[controller]")]
    [Authorize]
    public sealed class SitesController : Controller
    {
        private readonly ILogger<SitesController> _logger;
        private readonly ISiteService _service;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="service">Does the actual work, i.e. retrieves sites based on queries</param>
        public SitesController(
            ILogger<SitesController> logger,
            ISiteService service)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _service = service ?? throw new System.ArgumentNullException(nameof(service));
        }

        // GET api/sites/{guid}
        /// <summary> Returns a specific site by GUID </summary>
        /// <remarks>
        /// This API returns detailed information about the site, uniquely identified by its GUID.
        /// </remarks>
        /// <param name="guid">The site GUID</param>
        /// <returns>The Site identified by GUID.</returns>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "The site identified by ID.", Type = typeof(Resources.Site))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid GUID")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "If the site does not exist")]
        [HttpGet("{guid}")]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        public IActionResult Get(Guid guid)
        {
            var site = _service.GetSingle(guid);
            if (site == null)
            {
                return NotFound();
            }
            return Ok(site);
        }

        // GET api/sites
        /// <summary>
        /// Returns all sites in the system
        /// </summary>
        /// <remarks>
        /// This API is used to query all sites in the system. The query returns generic information about the sites.
        /// Each site can then be queried individually for more detailed information through the /api/Sites/{id} API.
        /// Various collection-type parameters can be specified to control
        /// the number of items to be returned or to request a specific page of the list.
        /// Examples:
        /// * Get all sites in the system (by default a maximum of 10 will be returned in the respose to this query)
        ///   (Additional ones can be requested by adjusting the page parameter):
        ///   /api/Sites
        /// * Get the sites in the system from the 11th to the 20th.
        ///   /api/Sites?page=2
        /// * Get the sites in the system, extending the maximum returned sites in this response to 30:
        ///   /api/Sites?pageCount=30
        /// </remarks>
        /// <param name="query">The request query parameters</param>
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Returns the sites", Type = typeof(Resources.CollectionResource<Resources.Site>))]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "No sites found matching the criteria")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid query parameters")]
        [HttpGet]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        public IActionResult Get([FromQuery] GetSitesRequestQuery query)
        {
            var sites = _service.Get(query);
            if (sites?.TotalCount == 0)
            {
                return NoContent();
            }
            return Ok(sites);
        }

        // POST api/sites
        /// <summary>
        /// Creates a new site
        /// </summary>
        /// <remarks>
        /// This API is used to create a new site. 
        /// Example:
        /// 
        ///     POST /api/sites
        ///     {
        ///        "name": "Site1",
        ///        "timezone": "CST"
        ///     }
        /// </remarks>
        /// <param name="site">The new site</param>
        /// <returns>The newly created resource</returns>
        [SwaggerResponse(StatusCodes.Status201Created, Description = "New site created successfully", Type = typeof(Resources.Site))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid query parameters")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, Description = "If the user does not have admin privileges")]
        [HttpPost]
        [ValidateActionParameters, ReturnBadRequestOnModelError]
        [Authorize(Roles = "admin")]
        public IActionResult Create([FromBody, Required] Resources.Site site)
        {
            var (uri, siteResource) = _service.Create(site);
            return Created(uri, siteResource);
        }

        // DELETE api/sites/{guid}
        /// <summary> Deletes a specific site by GUID </summary>
        /// <remarks>
        /// This API is used to delete a site.
        /// </remarks>
        /// <param name="guid">The site GUID</param>
        /// <returns>The deleted site.</returns>
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "The site has been deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid GUID")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, Description = "If the user does not have admin privileges")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "If the site does not exist")]
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
