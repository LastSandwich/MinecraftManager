namespace MapViewer.Services.SeedDatabase
{
    using MapViewer.Services.Enums;

    public interface ISeedData
    {
        public SeedType SeedType { get; }

        public Task Process();
    }
}
