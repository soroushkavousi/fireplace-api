using NLog;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Infrastructure.Extensions;

// Support Nlog Logger directly
public static class LogExtensions
{
    public static void LogAppTrace(this Logger logger, string message = null,
      Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.Trace(Application.Extensions.LogExtensions.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogAppInformation(this Logger logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.Info(Application.Extensions.LogExtensions.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogAppError(this Logger logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.Error(Application.Extensions.LogExtensions.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.Error(ex, Application.Extensions.LogExtensions.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogAppCritical(this Logger logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.Fatal(Application.Extensions.LogExtensions.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.Fatal(ex, Application.Extensions.LogExtensions.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }
}
