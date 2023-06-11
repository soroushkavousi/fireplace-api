using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Domain.Extensions;

public static class LogExtensions
{
    public static void LogAppTrace<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogTrace(CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogAppInformation<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogInformation(CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogAppWarning<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.LogWarning(CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.LogWarning(ex, CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogAppError<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.LogError(CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.LogError(ex, CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogAppCritical<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.LogCritical(CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.LogCritical(ex, CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

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
