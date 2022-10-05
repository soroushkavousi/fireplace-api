using System;

namespace FireplaceApi.Api.Tools
{
    public class EnvironmentVariable
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Default { get; set; }
        public bool IsProvided { get; set; }

        public static EnvironmentVariable EnvironmentName { get; } = new()
        {
            Key = "ASPNETCORE_ENVIRONMENT",
            Default = "Development"
        };
        public static EnvironmentVariable LogDirectory { get; } = new()
        {
            Key = "FIREPLACE_API_LOG_DIRECTORY",
            Default = $"{Utils.ContentRootPath}/Logs"
        };
        public static EnvironmentVariable ConnectionString { get; } = new()
        {
            Key = "FIREPLACE_API_CONNECTION_STRING",
            Default = ""
        };

        public void ReadValue()
        {
            IsProvided = true;
            Value = Environment.GetEnvironmentVariable(Key, EnvironmentVariableTarget.Process);
            if (!string.IsNullOrWhiteSpace(Value))
                return;

            Value = Environment.GetEnvironmentVariable(Key, EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrWhiteSpace(Value))
                return;

            Value = Environment.GetEnvironmentVariable(Key, EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(Value))
                return;

            IsProvided = false;
            Value = Default;
        }
    }
}
