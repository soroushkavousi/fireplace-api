using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Middlewares;

public class SampleGraphQLRequestMiddleware
{
    private readonly RequestDelegate _next;

    public SampleGraphQLRequestMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async ValueTask InvokeAsync(IRequestContext context)
    {
        await _next(context).ConfigureAwait(false);
    }
}

public static class SampleGraphQLRequestMiddlewareExntension
{
    public static IRequestExecutorBuilder UseSampleGraphQLRequestMiddleware(
        this IRequestExecutorBuilder builder) =>
        builder.UseRequest<SampleGraphQLRequestMiddleware>();
}
