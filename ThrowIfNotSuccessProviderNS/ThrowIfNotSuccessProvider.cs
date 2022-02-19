using CliExitCodeProviderNS;
using ExecuteCliCommandResultNS;

namespace ThrowIfNotSuccessProviderNS;

public static class ThrowIfNotSuccessProvider
{
    public static void ThrowIfNotSuccess(this ExecuteCliCommandResult executeCliCommandResult)
    {
        if (!executeCliCommandResult.ExitCode.IsSuccessfulCliExitCode())
        {
            throw new Exception(message: executeCliCommandResult.StandardErrorOutputText);
        }
    }
}