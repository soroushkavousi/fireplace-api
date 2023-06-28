using NLog;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Infrastructure.Loggers;

public static class NLogExtensions
{
    public static void LogServerTrace(this Logger logger, string message = null,
      Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.Trace(LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogServerInformation(this Logger logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        logger.Info(LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogServerError(this Logger logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.Error(LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.Error(ex, LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }

    public static void LogServerCritical(this Logger logger, string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        if (ex == null)
            logger.Fatal(LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        else
            logger.Fatal(ex, LogUtils.CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
    }
}
