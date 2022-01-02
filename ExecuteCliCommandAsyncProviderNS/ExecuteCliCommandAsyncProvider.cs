using System.CommandLine;
using System.Diagnostics;

namespace ExecuteCliCommandAsyncProviderNS;

public static class ExecuteCliCommandAsyncProvider
{
    public static async Task<int> ExecuteCliCommandAsync(
        string cliCommandText,
        IConsole console,
        CancellationToken cancellationToken
    )
    {
        var firstSpaceIndex = cliCommandText.IndexOf(' ');
        string fileName;
        string arguments;
        if (firstSpaceIndex != -1)
        {
            fileName = cliCommandText.Substring(0, firstSpaceIndex);
            arguments = cliCommandText.Substring(firstSpaceIndex);
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

        process.ErrorDataReceived += (sender, dataReceivedEventArgs) =>
        {
            console.Error.Write(dataReceivedEventArgs.Data);
        };
        process.OutputDataReceived += (sender, dataReceivedEventArgs) =>
        {
            console.Out.Write(dataReceivedEventArgs.Data);
        };

        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();

        await process.WaitForExitAsync(cancellationToken);
        return process.ExitCode;
    }
}