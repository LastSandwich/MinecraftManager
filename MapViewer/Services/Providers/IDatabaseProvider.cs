namespace MapViewer.Services.Providers
{
    using NPoco;

    public interface IDatabaseProvider<T>
    {
        IDatabase GetDatabase();
    }
}
