namespace Gproxyt;

internal static class LaunchFailureMessage
{
    public static string Format(Exception exception, string? logPath)
    {
        ArgumentNullException.ThrowIfNull(exception);
        var message = $"{AppLocalization.Current["LaunchFailed"]}\n\n{exception.Message}";
        return string.IsNullOrWhiteSpace(logPath) ? message : $"{message}\n\n{logPath}";
    }
}
