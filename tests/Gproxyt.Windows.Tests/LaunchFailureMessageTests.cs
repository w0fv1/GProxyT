namespace Gproxyt.Windows.Tests;

public sealed class LaunchFailureMessageTests
{
    [Fact]
    public void Startup_failure_includes_the_underlying_reason()
    {
        var exception = new InvalidOperationException("Window initialization failed.", new IOException("Access denied."));
        var message = LaunchFailureMessage.Format(exception, null, startup: true);

        Assert.StartsWith(AppLocalization.Current["StartupFailed"], message);
        Assert.Contains("Window initialization failed.", message);
        Assert.EndsWith("Access denied.", message);
    }

    [Fact]
    public void Preserves_the_failure_reason_and_log_location()
    {
        var message = LaunchFailureMessage.Format(
            new InvalidOperationException("ChatGPT did not create a visible window."),
            @"C:\logs\gproxyt.log");

        Assert.Contains("ChatGPT did not create a visible window.", message);
        Assert.Contains(@"C:\logs\gproxyt.log", message);
    }

    [Fact]
    public void Supports_failures_before_a_log_file_is_available()
    {
        var message = LaunchFailureMessage.Format(new InvalidOperationException("Activation failed."), null);

        Assert.EndsWith("Activation failed.", message);
    }
}
