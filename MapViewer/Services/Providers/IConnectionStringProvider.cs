namespace MapViewer.Services.Providers
{
    public interface IConnectionStringProvider
    {
        string MapViewerConnectionString { get; }

        string GetConnectionString(Type type);
    }
}
