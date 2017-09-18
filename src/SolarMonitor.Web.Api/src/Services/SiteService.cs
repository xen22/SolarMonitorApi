using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SolarMonitor.Data.Adapters;
using SolarMonitor.Data.Repositories;
using SolarMonitorApi.Helpers;
using SolarMonitorApi.RequestQueries;
using Models = SolarMonitor.Data.Models;
using Resources = SolarMonitor.Data.Resources;

namespace SolarMonitorApi.Services
{
    class SiteService : ISiteService
    {
        private readonly ILogger<SiteService> _logger;
        private readonly ISiteRepository _repository;
        private readonly ISiteAdapter _siteAdapter;


        public SiteService(
            ILogger<SiteService> logger,
            ISiteRepository repository,
            ISiteAdapter siteAdapter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _siteAdapter = siteAdapter ?? throw new ArgumentNullException(nameof(siteAdapter));
        }

        public Resources.CollectionResource<Resources.Site> Get(GetSitesRequestQuery query)
        {
            _logger.LogDebug("Getting Site based on query.");

            var sites = _repository.Sites(
                pageIndex: query.PageIndex ?? QueryParameterDefaults.PageIndex,
                pageSize: query.PageSize ?? QueryParameterDefaults.PageSize
            );

            var siteResources = sites.Select(s => _siteAdapter.ModelToResource(s));
            var collection = new Resources.CollectionResource<Resources.Site>(Resources.UriConstants.SitesPrefix,
                sites.PageIndex, sites.PageSize, sites.TotalCount, siteResources.ToList());

            return collection;
        }

        public Resources.Site GetSingle(Guid guid)
        {
            _logger.LogDebug($"Getting single Site with GUID {guid}");

            Models.Site site = _repository.Site(guid);
            if (site == null)
            {
                return null;
            }

            return _siteAdapter.ModelToResource(site);
        }

        public (string uri, Resources.Site) Create(Resources.Site site)
        {
            var siteModel = _siteAdapter.ResourceToModel(site);
            var savedSite = _repository.Create(siteModel);
            var siteResource = _siteAdapter.ModelToResource(savedSite);

            // TODO: find a way to add links seamlessly to Device resource
            siteResource.AddLink("self", new Resources.Link($"/api/sites/{siteResource.Guid}"));

            return (siteResource.Links[Resources.UriConstants.LinkKeySelf].HRef, siteResource);
        }

        public bool Delete(Guid guid)
        {
            return _repository.Delete(guid);
        }
    }
}
