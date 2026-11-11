namespace Gproxyt;

internal static class LaunchFailureMessage
{
    public static string Format(Exception exception, string? logPath, bool startup = false)
    {
        ArgumentNullException.ThrowIfNull(exception);
        var heading = AppLocalization.Current[startup ? "StartupFailed" : "LaunchFailed"];
        var message = $"{heading}\n\n{exception.Message}";
        var cause = exception.GetBaseException();
        if (cause.Message != exception.Message)
        {
            message += $"\n{cause.Message}";
        }
        return string.IsNullOrWhiteSpace(logPath) ? message : $"{message}\n\n{logPath}";
    }
}
