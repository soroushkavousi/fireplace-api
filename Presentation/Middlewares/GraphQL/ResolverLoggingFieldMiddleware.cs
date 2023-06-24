using FireplaceApi.Presentation.Tools;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Middlewares;

public class ResolverLoggingFieldMiddleware
{
    private readonly FieldDelegate _next;
    private readonly ILogger<ResolverLoggingFieldMiddleware> _logger;

    public ResolverLoggingFieldMiddleware(FieldDelegate next,
        ILogger<ResolverLoggingFieldMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(IMiddlewareContext context)
    {
        if (!context.IsApiResolver())
        {
            await _next(context);
            return;
        }
        var sw = Stopwatch.StartNew();
        var inputs = context.GetResolverInputs();
        var path = context.Path.ToString();
        _logger.LogAppInformation(message: path, title: "RESOLVER_INPUT", parameters: inputs);
        await _next(context);
        _logger.LogAppInformation(sw: sw, message: path, title: "RESOLVER_OUTPUT", parameters: context.Result);
    }
}

public static class ResolverLoggingFieldMiddlewareExtensions
{
    internal static Dictionary<string, object> GetResolverInputs(this IMiddlewareContext context)
    {
        var arguments = (IReadOnlyDictionary<string, ArgumentValue>)context.GetType().GetProperty("Arguments").GetValue(context, null);
        var argumentKeys = arguments.Keys;
        var inputs = new Dictionary<string, object>();
        foreach (var key in argumentKeys)
        {
            var value = context.ArgumentValue<object>(key);
            inputs.Add(key, value);
        }
        return inputs;
    }
}
