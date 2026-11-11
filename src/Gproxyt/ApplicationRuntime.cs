using Gproxyt.Core;

namespace Gproxyt;

internal sealed class ApplicationRuntime
{
    private readonly LauncherSettingsStore settingsStore = AppPaths.CreateSettingsStore();
    private readonly IApplicationLog log;
    private readonly IChatGptInstallationLocator installationLocator;
    private readonly GproxytLauncher launcher;
    private readonly IStartupRegistration startupRegistration = StartupRegistration.Create();

    public ApplicationRuntime(IApplicationLog log)
    {
        this.log = log;
        installationLocator = new ChatGptInstallationLocator(new WindowsPackageRegistrationSource(log));
        launcher = new GproxytLauncher(installationLocator, new WindowsProcessManager(log));
        Diagnostics = new EnvironmentDiagnostics(installationLocator);
    }

    public EnvironmentDiagnostics Diagnostics { get; }

    public string FormatLaunchFailure(Exception exception) => LaunchFailureMessage.Format(exception, log.FilePath);

    public LauncherSettings LoadSettings()
    {
        var settings = settingsStore.Load();
        log.Information(
            "settings_loaded",
            ("RestartExisting", settings.RestartExisting),
            ("StartWithWindows", settings.StartWithWindows));
        return settings;
    }

    public Task SynchronizeStartupAsync() => startupRegistration.ApplyAsync(LoadSettings().StartWithWindows);

    public async Task<LauncherSettings> SaveSettingsAsync(LauncherSettings settings)
    {
        var normalized = settings.Normalize();
        await startupRegistration.ApplyAsync(normalized.StartWithWindows);
        settingsStore.Save(normalized);
        log.Information(
            "settings_saved",
            ("RestartExisting", normalized.RestartExisting),
            ("StartWithWindows", normalized.StartWithWindows));
        return normalized;
    }

    public async Task<LaunchResult> LaunchAsync(LauncherSettings settings)
    {
        var elapsed = System.Diagnostics.Stopwatch.StartNew();
        var stage = "save_settings";
        try
        {
            log.Information("launch_requested");
            var normalized = await SaveSettingsAsync(settings);
            stage = "parse_proxy";
            var proxy = ProxyEndpoint.Parse(normalized.ProxyUrl);
            var uri = new Uri(proxy.Value);
            log.Information(
                "launch_started",
                ("ProxyScheme", uri.Scheme),
                ("ProxyHost", uri.Host),
                ("ProxyPort", uri.Port),
                ("RestartExisting", normalized.RestartExisting));
            stage = "check_proxy";
            await Diagnostics.EnsureProxyReachableAsync(proxy);
            log.Information("proxy_reachable");
            stage = "launch_package";
            var result = await launcher.LaunchAsync(normalized);
            log.Information(
                "launch_completed",
                ("PackageFullName", result.Installation.PackageFullName),
                ("AppUserModelId", result.Installation.AppUserModelId),
                ("ProcessId", result.ProcessId));
            return result;
        }
        catch (Exception exception)
        {
            log.Error(exception, "launch_failed", ("Stage", stage), ("ElapsedMilliseconds", elapsed.ElapsedMilliseconds), ("HResult", exception.HResult));
            throw;
        }
    }

    public string CreateShortcut()
    {
        var installation = installationLocator.Locate();
        var path = ShortcutService.Create(installation.ExecutablePath);
        log.Information("shortcut_created", ("Path", path));
        return path;
    }
}
