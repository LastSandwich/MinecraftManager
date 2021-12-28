namespace MapViewer.Services.Processors
{
    public interface IProcessTask
    {
        int Order { get; }

        string Name { get; }

        TaskType TaskType { get; }

        Task<ProcessResult> Run();
    }
}
