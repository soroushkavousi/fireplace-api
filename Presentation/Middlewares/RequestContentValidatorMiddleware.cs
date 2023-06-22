using FireplaceApi.Presentation.Exceptions;
using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Application.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Middlewares;

public class RequestContentValidatorMiddleware
{
    private readonly ILogger<RequestContentValidatorMiddleware> _logger;
    private readonly RequestDelegate _next;

    public RequestContentValidatorMiddleware(ILogger<RequestContentValidatorMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var sw = Stopwatch.StartNew();
        await ControlRequestBody(httpContext);
        await _next(httpContext);
        _logger.LogAppInformation(sw: sw, title: "REQUEST_CONTENT_VALIDATOR_MIDDLEWARE");
    }

    private async Task ControlRequestBody(HttpContext httpContext)
    {
        var httpMethod = new HttpMethod(httpContext.Request.Method);
        if (httpMethod == HttpMethod.Post
            || httpMethod == HttpMethod.Put
            || httpMethod == HttpMethod.Patch)
        {
            CheckRequestContentType(httpContext.Request);

            if (httpContext.Request.ContentType.Contains("application/json")
                || httpContext.Request.ContentType.Contains("application/merge-patch+json"))
            {
                var requestBody = await httpContext.Request.ReadRequestBodyAsync();
                ValidateRequestBodyIsJson(requestBody);
            }
        }
    }

    public void CheckRequestContentType(HttpRequest request)
    {
        if (request.ContentType == null
            || (
                request.ContentType.Contains("application/json") == false
                && request.ContentType.Contains("multipart/form-data") == false
                && request.ContentType.Contains("application/merge-patch+json") == false
                ))
            throw new RequestContentTypeIncorrectValueException(request.ContentType);
    }

    public void ValidateRequestBodyIsJson(string requestJsonBody)
    {
        if (requestJsonBody.IsJson() == false)
            throw new RequestBodyInvalidFormatException();
    }
}

public static class IApplicationBuilderRequestContentValidatorMiddleware
{
    public static IApplicationBuilder UseRequestContentValidator(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestContentValidatorMiddleware>();
    }
}
