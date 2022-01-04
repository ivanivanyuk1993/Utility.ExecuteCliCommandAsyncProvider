namespace ExecuteCliCommandResultNS;

public class ExecuteCliCommandResult
{
    public ExecuteCliCommandResult(
        int exitCode,
        string standardOutputText
    )
    {
        ExitCode = exitCode;
        StandardOutputText = standardOutputText;
    }

    public int ExitCode { get; }
    public string StandardOutputText { get; }
}