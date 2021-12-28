namespace MapViewer.Services.Data.AdminService
{
    using MapViewer.Services.Dtos;
    using MapViewer.Services.Providers;

    using NPoco;

    public class MapViewerAdminService : IMapViewerAdminService
    {
        private readonly IDatabase _database;

        public MapViewerAdminService(IDatabaseProvider<IWorldDatabase> databaseProvider)
        {
            _database = databaseProvider.GetDatabase();
        }

        public async Task<IEnumerable<World>> GetWorlds()
        {
            return await _database.QueryAsync<World>().ToList();
        }
    }
}
