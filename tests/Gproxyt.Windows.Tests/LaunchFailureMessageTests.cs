namespace Gproxyt.Windows.Tests;

public sealed class LaunchFailureMessageTests
{
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
