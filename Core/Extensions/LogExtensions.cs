using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Extensions
{
    public static class LogExtensions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void LogTrace(this Stopwatch sw, string message = "") => _logger.LogTrace(sw, message.ShowExecutionTimeTagIfMessageIsEmpty());
        public static void LogInformation(this Stopwatch sw, string message = "") => _logger.LogInformation(sw, message.ShowExecutionTimeTagIfMessageIsEmpty());

        public static long End(this Stopwatch stopWatch)
        {
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }

        public static void LogTrace<T>(this ILogger<T> logger, Stopwatch sw, string message = "")
        {
            logger.LogTrace($"{{executionTime}}ms | {message.ShowExecutionTimeTagIfMessageIsEmpty()}", sw.End());
        }

        public static void LogInformation<T>(this ILogger<T> logger, Stopwatch sw, string message = "")
        {
            logger.LogInformation($"{{executionTime}}ms | {message.ShowExecutionTimeTagIfMessageIsEmpty()}", sw.End());
        }

        public static void LogTrace(this Logger logger, Stopwatch sw, string message)
        {
            logger.Trace($"{{executionTime}}ms | {message.ShowExecutionTimeTagIfMessageIsEmpty()}", sw.End());
        }

        public static void LogInformation(this Logger logger, Stopwatch sw, string message)
        {
            logger.Info($"{{executionTime}}ms | {message.ShowExecutionTimeTagIfMessageIsEmpty()}", sw.End());
        }

        private static string ShowExecutionTimeTagIfMessageIsEmpty(this string message)
        {
            return message.IsNullOrEmpty() ? "#ExecutionTime" : message.Replace("{", "{{").Replace("}", "}}");
        }

        //private static string AddMessageIfNotEmpty(this string title, string message)
        //{
        //    return title + $"{(message.IsNullOrEmpty() ? " | " : "")}{message}";
        //}
    }
}
