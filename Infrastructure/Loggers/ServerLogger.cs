using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Infrastructure.Loggers;

public class ServerLogger<T> : IServerLogger<T> where T : class
{
    private readonly ILogger<T> _logger;

    public ServerLogger(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogServerTrace(string message = null, Stopwatch sw = null,
        string title = null, object parameters = null, [CallerFilePath] string sourceFilePath = "",
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        => _logger.LogServerTrace(message, sw, title, parameters, sourceFilePath,
                memberName, sourceLineNumber);

    public void LogServerInformation(string message = null, Stopwatch sw = null,
        string title = null, object parameters = null, [CallerFilePath] string sourceFilePath = "",
        [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        => _logger.LogServerInformation(message, sw, title, parameters, sourceFilePath,
                memberName, sourceLineNumber);

    public void LogServerWarning(string message = null, Stopwatch sw = null,
        string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        => _logger.LogServerWarning(message, sw, title, parameters, ex, sourceFilePath,
                memberName, sourceLineNumber);

    public void LogServerError(string message = null, Stopwatch sw = null,
        string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        => _logger.LogServerError(message, sw, title, parameters, ex, sourceFilePath,
                memberName, sourceLineNumber);

    public void LogServerCritical(string message = null, Stopwatch sw = null,
        string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        => _logger.LogServerCritical(message, sw, title, parameters, ex, sourceFilePath,
                memberName, sourceLineNumber);
}
