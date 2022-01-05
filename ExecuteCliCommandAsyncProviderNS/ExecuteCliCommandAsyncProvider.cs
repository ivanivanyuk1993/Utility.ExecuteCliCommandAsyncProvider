using System.CommandLine;
using System.CommandLine.IO;
using System.Diagnostics;
using ExecuteCliCommandResultNS;

namespace ExecuteCliCommandAsyncProviderNS;

public static class ExecuteCliCommandAsyncProvider
{
    public static async Task<ExecuteCliCommandResult> ExecuteCliCommandAsync(
        string cliCommandText,
        IConsole console,
        CancellationToken cancellationToken
    )
    {
        var firstSpaceIndex = cliCommandText.IndexOf(value: ' ');
        string fileName;
        string arguments;
        if (firstSpaceIndex != -1)
        {
            fileName = cliCommandText.Substring(startIndex: 0, length: firstSpaceIndex);
            arguments = cliCommandText.Substring(startIndex: firstSpaceIndex);
        }
        else
        {
            fileName = cliCommandText;
            arguments = "";
        }

        using var process = new Process
        {
            StartInfo =
            {
                Arguments = arguments,
                FileName = fileName,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            }
        };

        var standardErrorOutputTextList = new List<string?>();
        var standardOutputTextList = new List<string?>();

        process.ErrorDataReceived += (sender, dataReceivedEventArgs) =>
        {
            standardErrorOutputTextList.Add(item: dataReceivedEventArgs.Data);
            console.Error.WriteLine(value: dataReceivedEventArgs.Data);
        };
        process.OutputDataReceived += (sender, dataReceivedEventArgs) =>
        {
            standardOutputTextList.Add(item: dataReceivedEventArgs.Data);
            console.Out.WriteLine(value: dataReceivedEventArgs.Data);
        };

        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();

        await process.WaitForExitAsync(cancellationToken: cancellationToken);
        return new ExecuteCliCommandResult(
            exitCode: process.ExitCode,
            standardErrorOutputTextList: standardErrorOutputTextList,
            standardOutputTextList: standardOutputTextList
        );
    }
}