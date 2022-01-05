namespace ExecuteCliCommandResultNS;

public class ExecuteCliCommandResult
{
    public ExecuteCliCommandResult(
        int exitCode,
        List<string?> standardErrorOutputTextList,
        List<string?> standardOutputTextList
    )
    {
        ExitCode = exitCode;
        StandardErrorOutputTextList = standardErrorOutputTextList;
        StandardOutputTextList = standardOutputTextList;
    }

    public int ExitCode { get; }
    public string StandardErrorOutputText => string.Join(
        separator: '\n',
        values: StandardErrorOutputTextList
    );
    public List<string?> StandardErrorOutputTextList { get; }
    public List<string?> StandardOutputTextList { get; }
}