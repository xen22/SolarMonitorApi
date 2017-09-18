namespace SolarMonitor.Data.Adapters
{
    public interface IDataAdapter<Model, Resource>
    {
        Resource ModelToResource(Model model);
        Model ResourceToModel(Resource resource);
    }
}
