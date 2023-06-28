using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Infrastructure.Loggers;

public static class ILoggerExtensions
{
    public static void LogServerTrace<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogTrace(LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogServerInformation<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.LogInformation(LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogServerWarning<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.LogWarning(LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.LogWarning(ex, LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogServerError<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.LogError(LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.LogError(ex, LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogServerCritical<T>(this ILogger<T> logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.LogCritical(LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.LogCritical(ex, LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

}
