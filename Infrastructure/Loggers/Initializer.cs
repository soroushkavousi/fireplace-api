using NLog;
using NLog.Web;
using System;

namespace FireplaceApi.Infrastructure.Loggers;

public static class Initializer
{
    private const string _logRootDirectoryPathKey = "logRootDirectoryPath";

    public static void Initialize(string configFilePath, string logRootDirectoryPath)
    {
        try
        {
            NLogBuilder.ConfigureNLog(configFilePath);
            LogManager.Configuration.Variables[_logRootDirectoryPathKey] = logRootDirectoryPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't setup logger: {ex}");
            throw;
        }
    }
}
