using System.CommandLine;
using System.CommandLine.IO;
using System.Diagnostics;
using System.Text;
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

        var standardOutputStringBuilder = new StringBuilder();

        process.ErrorDataReceived += (sender, dataReceivedEventArgs) =>
        {
            console.Error.WriteLine(value: dataReceivedEventArgs.Data);
        };
        process.OutputDataReceived += (sender, dataReceivedEventArgs) =>
        {
            standardOutputStringBuilder.Append(value: dataReceivedEventArgs.Data);
            console.Out.WriteLine(value: dataReceivedEventArgs.Data);
        };

        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();

        await process.WaitForExitAsync(cancellationToken: cancellationToken);
        return new ExecuteCliCommandResult(
            exitCode: process.ExitCode,
            standardOutputText: standardOutputStringBuilder.ToString()
        );
    }
}