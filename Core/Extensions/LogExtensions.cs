using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Core.Extensions
{
    public static class LogExtensions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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

        public static void LogAppTrace(this Logger logger, string message = null,
          Stopwatch sw = null, string title = null, object parameters = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.Trace(CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        }

        public static void LogAppInformation(this Logger logger, string message = null,
            Stopwatch sw = null, string title = null, object parameters = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.Info(CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        }

        public static void LogAppError(this Logger logger, string message = null,
            Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (ex == null)
                logger.Error(CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
            else
                logger.Error(ex, CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
        }

        public static void LogAppCritical(this Logger logger, string message = null,
            Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
            [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (ex == null)
                logger.Fatal(CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
            else
                logger.Fatal(ex, CreateLogMessage(sourceFilePath, memberName, sourceLineNumber, message, sw, title, parameters));
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
                logMessage += $" | {parameters.ToJson()}";
            }

            //logMessage = logMessage.EscapeCurlyBrackets();
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
}
