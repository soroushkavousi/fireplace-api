using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FireplaceApi.Application.Tools
{
    public class EnvironmentVariable
    {
        private readonly static IEnumerable<KeyValuePair<string, string>> _launchVariables = null;

        public string Key { get; set; }
        public string Value { get; set; }
        public string Default { get; set; }
        public bool IsProvided { get; set; }

        static EnvironmentVariable()
        {
            var profilePath = Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json");
            if (File.Exists(profilePath))
                _launchVariables = new ConfigurationBuilder()
                    .AddJsonFile(profilePath, optional: false, reloadOnChange: false)
                    .Build().AsEnumerable();
        }

        public static EnvironmentVariable EnvironmentName { get; } = new()
        {
            Key = "ASPNETCORE_ENVIRONMENT",
            Default = "Development"
        };
        public static EnvironmentVariable LogDirectory { get; } = new()
        {
            Key = "FIREPLACE_API_LOG_DIRECTORY",
            Default = Path.Combine(Utils.ContentRootPath, "Logs")
        };
        public static EnvironmentVariable ConnectionString { get; } = new()
        {
            Key = "FIREPLACE_API_CONNECTION_STRING",
            Default = ""
        };

        public void ReadValue()
        {
            IsProvided = true;

            if (_launchVariables != null)
            {
                var theVariable = _launchVariables.FirstOrDefault(v => v.Key.Contains(Key, StringComparison.OrdinalIgnoreCase));
                if (!theVariable.Equals(default(KeyValuePair<string, string>)))
                {
                    Value = theVariable.Value;
                    if (!string.IsNullOrWhiteSpace(Value))
                        return;
                }
            }

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
