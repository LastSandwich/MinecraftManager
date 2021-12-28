namespace MapViewer.Services.Data.AdminService
{
    using MapViewer.Services.Dtos;

    public interface IMapViewerAdminService
    {
        Task<IEnumerable<World>> GetWorlds();
    }
}
