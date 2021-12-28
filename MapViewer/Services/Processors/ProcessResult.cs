namespace MapViewer.Services.Processors;

public class ProcessResult
{
    public bool IsSuccess { get; private init; } = false;

    public string Message { get; private init; } = string.Empty;

    public static ProcessResult Success()
    {
        return new ProcessResult { IsSuccess = true };
    }

    public static ProcessResult Error(string message)
    {
        return new ProcessResult { IsSuccess = false, Message = message };
    }
}
