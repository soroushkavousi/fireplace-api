using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Application.Operators;
using HotChocolate;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Middlewares;

public class ApiExceptionFieldMiddleware
{
    private readonly FieldDelegate _next;
    private readonly ILogger<ApiExceptionFieldMiddleware> _logger;
    private readonly ErrorOperator _errorOperator;

    public ApiExceptionFieldMiddleware(FieldDelegate next,
        ILogger<ApiExceptionFieldMiddleware> logger,
        ErrorOperator errorOperator)
    {
        _next = next;
        _logger = logger;
        _errorOperator = errorOperator;
    }

    public async Task InvokeAsync(IMiddlewareContext context)
    {
        if (!context.IsApiResolver())
        {
            await _next(context);
            return;
        }
        var sw = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        catch (GraphQLException) { throw; }
        catch (Exception ex)
        {
            var error = await _errorOperator.GetErrorAsync(ex);
            var graphQLError = new Error(
                message: error.ClientMessage,
                code: error.Code.ToString(),
                extensions: new Dictionary<string, object>
                {
                    ["type"] = error.Type.Name,
                    ["field"] = error.Field.Name,
                });
            throw new GraphQLException(graphQLError);
        }
    }
}
