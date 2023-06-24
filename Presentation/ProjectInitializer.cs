using FireplaceApi.Infrastructure.Extensions;
using FireplaceApi.Presentation.Tools;
using NLog;

namespace FireplaceApi.Presentation;

public static class ProjectInitializer
{
    private static bool _initialized = false;
    private static Logger _logger;

    public static Logger Logger { get; private set; }

    public static void Start()
    {
        if (_initialized)
            return;

        _initialized = true;
        AppSettings.Initialize(EnvironmentVariable.EnvironmentName.Value);
        Infrastructure.ProjectInitializer.Initialize(
            AppSettings.LogConfigFilePath.Value,
            EnvironmentVariable.LogDirectory.Value,
            EnvironmentVariable.ConnectionString.Value,
            EnvironmentVariable.EnvironmentName.Value);
        _logger = LogManager.GetCurrentClassLogger();
        _logger.LogAppInformation($"Project '{nameof(Presentation)}' has initialized successfully.");
    }
}
