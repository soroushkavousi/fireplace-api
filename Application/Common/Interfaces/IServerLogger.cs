using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Application.Common;

public interface IServerLogger<T> where T : class
{
    public void LogServerTrace(string message = null,
        Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    public void LogServerInformation(string message = null,
        Stopwatch sw = null, string title = null, object parameters = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    public void LogServerWarning(string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    public void LogServerError(string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    public void LogServerCritical(string message = null,
        Stopwatch sw = null, string title = null, object parameters = null, Exception ex = null,
        [CallerFilePath] string sourceFilePath = "", [CallerMemberName] string memberName = "",
        [CallerLineNumber] int sourceLineNumber = 0);
}
