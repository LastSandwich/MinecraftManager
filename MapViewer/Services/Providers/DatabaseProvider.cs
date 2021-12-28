namespace MapViewer.Services.Providers
{
    using NPoco;
    using NPoco.SqlServer;

    public class DatabaseProvider<T> : IDatabaseProvider<T>
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public DatabaseProvider(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IDatabase GetDatabase()
        {
            var connectionString = _connectionStringProvider.GetConnectionString(typeof(T));
            return new SqlServerDatabase(connectionString);
        }
    }
}
