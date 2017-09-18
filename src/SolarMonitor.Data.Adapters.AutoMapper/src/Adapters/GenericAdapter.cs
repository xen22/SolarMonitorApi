using Resources = SolarMonitor.Data.Resources;
using Models = SolarMonitor.Data.Models;
using AutoMapper;

namespace SolarMonitor.Data.Adapters.AutoMapper
{
    public abstract class GenericAdapter<TModel, TResource>
    {
        private readonly IMapper _mapper;

        protected GenericAdapter(IMapper mapper)
        {
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        public TResource ModelToResource(TModel model)
        {
            return _mapper.Map<TModel, TResource>(model);
        }
        public TModel ResourceToModel(TResource resource)
        {
            return _mapper.Map<TResource, TModel>(resource);
        }
    }

}