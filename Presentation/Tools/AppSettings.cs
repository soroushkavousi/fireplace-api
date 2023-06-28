using Microsoft.Extensions.Configuration;
using System.IO;

namespace FireplaceApi.Presentation.Tools;

public class AppSettings
{
    private static IConfigurationRoot _appSettings;

    public string Key { get; private set; }
    public string Value { get; private set; }
    public string Default { get; private set; }

    public AppSettings(string key, string @default)
    {
        Key = key;
        Default = @default;
    }

    public static void Initialize(string environmentName)
    {
        _appSettings = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: false)
            .Build();

        LogConfigFilePath.ReadValue();
    }

    private void ReadValue()
    {
        Value = _appSettings[Key];
        if (string.IsNullOrWhiteSpace(Value))
            Value = Default;
    }

    public static AppSettings LogConfigFilePath { get; } = new
    (
        key: "Log:ConfigFilePath",
        @default: Path.Combine(Utils.ContentRootPath, "Loggers", "nlog.config")
    );
}
