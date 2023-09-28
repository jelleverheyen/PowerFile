using Microsoft.Extensions.Options;

namespace PowerFile.CommandLine.Config;

public abstract class CrossPlatformDirectories(string applicationName)
{
    public abstract string Data { get; }
    public abstract string Config { get; }
    public abstract string Cache { get; }
    public abstract string Log { get; }
    public abstract string Temp { get; }
}

public class LinuxPowerFileDirectories(string applicationName) : CrossPlatformDirectories(applicationName)
{
    private readonly IDictionary<string, string> _map = new Dictionary<string, string>()
    {
        { nameof(Data), Path.Combine(Environment.GetEnvironmentVariable("XDG_DATA_HOME") ?? "~/.local/share", applicationName)},
        { nameof(Config), Path.Combine(Environment.GetEnvironmentVariable("XDG_CONFIG_HOME") ?? "~/.config", applicationName )},
        { nameof(Cache), Path.Combine(Environment.GetEnvironmentVariable("XDC_CACHE_HOME") ?? "~/.cache", applicationName )},
        { nameof(Log), Path.Combine(Environment.GetEnvironmentVariable("XDC_STATE_HOME") ?? "~/.local/state", applicationName )},
        { nameof(Temp), Path.Combine(Path.GetTempPath(), applicationName) },
    };

    public override string Data => _map[nameof(Data)];
    public override string Config => _map[nameof(Config)];
    public override string Cache => _map[nameof(Cache)];
    public override string Log => _map[nameof(Log)];
    public override string Temp => _map[nameof(Temp)];
}

public class MacPowerFileDirectories(string applicationName) : CrossPlatformDirectories(applicationName)
{
    private readonly IDictionary<string, string> _map = new Dictionary<string, string>()
    {
        { nameof(Data), Path.Combine("~/Library/Application Support", applicationName )},
        { nameof(Config), Path.Combine("~/Library/Preferences", applicationName )},
        { nameof(Cache), Path.Combine("~/Library/Caches", applicationName )},
        { nameof(Log), Path.Combine("~/Library/Logs", applicationName )},
        { nameof(Temp), Path.Combine(Path.GetTempPath(), applicationName) }
    };

    public override string Data => _map[nameof(Data)];
    public override string Config => _map[nameof(Config)];
    public override string Cache => _map[nameof(Cache)];
    public override string Log => _map[nameof(Log)];
    public override string Temp => _map[nameof(Temp)];
}

public class WindowsPowerFileDirectories(string applicationName) : CrossPlatformDirectories(applicationName)
{
    private readonly IDictionary<string, string> _map = new Dictionary<string, string>()
    {
        { nameof(Data), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data")},
        { nameof(Config), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), applicationName, "Config")},
        { nameof(Cache), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), applicationName, "Cache" )},
        { nameof(Log), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), applicationName, "Log")},
        { nameof(Temp), Path.Combine(Path.GetTempPath(), applicationName) }
    };

    public override string Data => _map[nameof(Data)];
    public override string Config => _map[nameof(Config)];
    public override string Cache => _map[nameof(Cache)];
    public override string Log => _map[nameof(Log)];
    public override string Temp => _map[nameof(Temp)];
}