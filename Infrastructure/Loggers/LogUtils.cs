using FireplaceApi.Infrastructure.Serializers;
using System.Diagnostics;

namespace FireplaceApi.Infrastructure.Loggers;

public static class LogUtils
{
    public static string CreateLogMessage(string sourceFilePath, string memberName,
        int sourceLineNumber, string message = null, Stopwatch sw = null,
        string title = null, object parameters = null)
    {
        var logMessage = string.Empty;

        var fileName = sourceFilePath.ExtractFileNameWithoutExtension();
        var codeLocationInfo = $"{fileName}.{memberName} {sourceLineNumber}";
        logMessage += $"{codeLocationInfo,-64}";

        var executionTime = sw.Finish();
        logMessage += $" | {executionTime,4}ms";

        logMessage += $" | {title,-21}";

        if (message != null)
            logMessage += $" | {message}";

        if (parameters != null)
        {
            var json = parameters.ToJson();
            var maxJsonSize = 300;
            if (json.Length > maxJsonSize)
                json = json[..maxJsonSize] + "...";
            logMessage += $" | {json}";
        }

        return logMessage;
    }

    public static long Finish(this Stopwatch sw)
    {
        if (sw == null)
            return 0;
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }
}
