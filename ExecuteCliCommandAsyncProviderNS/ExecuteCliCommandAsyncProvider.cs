using System.CommandLine;
using System.CommandLine.IO;
using System.Diagnostics;

namespace ExecuteCliCommandAsyncProviderNS;

public static class ExecuteCliCommandAsyncProvider
{
    public static async Task<int> ExecuteCommandAsync(
        string command,
        IConsole console,
        CancellationToken cancellationToken
    )
    {
        var firstSpaceIndex = command.IndexOf(' ');
        string fileName;
        string arguments;
        if (firstSpaceIndex != -1)
        {
            fileName = command.Substring(0, firstSpaceIndex);
            arguments = command.Substring(firstSpaceIndex);
        }
        else
        {
            fileName = command;
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
            console.Error.WriteLine(dataReceivedEventArgs.Data);
        };
        process.OutputDataReceived += (sender, dataReceivedEventArgs) =>
        {
            console.Out.WriteLine(dataReceivedEventArgs.Data);
        };

        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();

        await process.WaitForExitAsync(cancellationToken);
        return process.ExitCode;
    }
}