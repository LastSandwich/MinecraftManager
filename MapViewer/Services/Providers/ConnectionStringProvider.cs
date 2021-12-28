namespace MapViewer.Services.Providers
{
    using MapViewer.Services.Dtos;

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly Dictionary<Type, string> _connectionStrings;

        public ConnectionStringProvider(string connectionString)
        {
            MapViewerConnectionString = connectionString;
            _connectionStrings = new Dictionary<Type, string>
            {
                { typeof(IWorldDatabase), MapViewerConnectionString }, { typeof(IOtherDatabase), MapViewerConnectionString },
            };
        }

        public string MapViewerConnectionString { get; }

        public string GetConnectionString(Type type)
        {
            return _connectionStrings[type];
        }
    }
}
