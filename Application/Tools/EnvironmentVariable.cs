using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FireplaceApi.Application.Tools
{
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

            IsProvided = TryReadValueFromLaunchVariables();
            if (IsProvided)
                return;

            IsProvided = TryReadValueFromSystem();
            if (IsProvided)
                return;

            Value = Default;
        }

        public static EnvironmentVariable EnvironmentName { get; } = new
        (
            key: "ASPNETCORE_ENVIRONMENT",
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

        private bool TryReadValueFromLaunchVariables()
        {
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json");
            if (!File.Exists(profilePath))
                return false;

            var launchVariables = new ConfigurationBuilder()
                .AddJsonFile(profilePath, optional: false, reloadOnChange: false)
                .Build().AsEnumerable();

            var theVariable = launchVariables.FirstOrDefault(v => v.Key.Contains(Key, StringComparison.OrdinalIgnoreCase));
            if (!theVariable.Equals(default(KeyValuePair<string, string>)))
            {
                Value = theVariable.Value;
                if (!string.IsNullOrWhiteSpace(Value))
                    return true;
            }

            return false;
        }

        private bool TryReadValueFromSystem()
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
}
