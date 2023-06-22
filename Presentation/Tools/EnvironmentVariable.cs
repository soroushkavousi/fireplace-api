using System;
using System.IO;

namespace FireplaceApi.Presentation.Tools;

public class EnvironmentVariable
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string Default { get; set; }
    public bool IsProvided { get; set; }

    public EnvironmentVariable(string key, string @default)
    {
        Key = key;
        Default = @default;

        IsProvided = TryReadValue();
        if (IsProvided)
            return;

        Value = Default;
    }

    public static EnvironmentVariable EnvironmentName { get; } = new
    (
        key: "FIREPLACE_API_ASPNETCORE_ENVIRONMENT",
        @default: "Development"
    );

    public static EnvironmentVariable LogDirectory { get; } = new
    (
        key: "FIREPLACE_API_LOG_DIRECTORY",
        @default: Path.Combine(Utils.ContentRootPath, "Logs")
    );

    public static EnvironmentVariable ConnectionString { get; } = new
    (
        key: "FIREPLACE_API_CONNECTION_STRING",
        @default: ""
    );

    private bool TryReadValue()
    {
        Value = Environment.GetEnvironmentVariable(Key, EnvironmentVariableTarget.Process);
        if (!string.IsNullOrWhiteSpace(Value))
            return true;

        Value = Environment.GetEnvironmentVariable(Key, EnvironmentVariableTarget.Machine);
        if (!string.IsNullOrWhiteSpace(Value))
            return true;

        Value = Environment.GetEnvironmentVariable(Key, EnvironmentVariableTarget.User);
        if (!string.IsNullOrWhiteSpace(Value))
            return true;

        return false;
    }
}
