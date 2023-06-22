using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Presentation.Tools;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Extensions;

public static class HttpContextExtensions
{
    private static readonly string _inputHeaderDtoKey = "InputHeaderDto";
    private static readonly string _inputCookieDtoKey = "InputCookieDto";
    private static readonly string _accessTokenKey = "AccessToken";

    public static InputHeaderDto GetInputHeaderDto(this HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue(_inputHeaderDtoKey, out object value))
            return value.To<InputHeaderDto>();

        var inputHeaderDto = new InputHeaderDto(httpContext);
        httpContext.Items[_inputHeaderDtoKey] = inputHeaderDto;
        return inputHeaderDto;
    }

    public static InputCookieDTO GetInputCookieDto(this HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue(_inputCookieDtoKey, out object value))
            return value.To<InputCookieDTO>();

        var inputCookieDto = new InputCookieDTO(httpContext);
        httpContext.Items[_inputCookieDtoKey] = inputCookieDto;
        return inputCookieDto;
    }

    public static string GetAccessTokenValue(this HttpContext httpContext)
    {
        if (httpContext.Items.TryGetValue(_accessTokenKey, out object value))
            return value.To<string>();

        var inputHeaderDto = httpContext.GetInputHeaderDto();
        var inputCookieDto = httpContext.GetInputCookieDto();

        var accessTokenValue = inputHeaderDto.AccessTokenValue;
        if (string.IsNullOrWhiteSpace(accessTokenValue))
            accessTokenValue = inputCookieDto.AccessTokenValue;

        httpContext.Items[_accessTokenKey] = accessTokenValue;
        return accessTokenValue;
    }

    public static Error GetError(this HttpContext httpContext)
    {
        Error error = null;
        var hasError = httpContext.Items.TryGetValue(Constants.ErrorKey, out var errorObject);
        if (hasError)
            error = errorObject.To<Error>();
        return error;
    }

    public static async Task<string> ReadRequestBodyAsync(this HttpRequest request)
    {
        request.EnableBuffering();

        request.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(request.Body, encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var requestBodyText = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);

        return requestBodyText;
    }

    public static T GetActionAttribute<T>(this HttpContext httpContext)
        where T : class
    {
        var endpoint = httpContext.Request.HttpContext.GetEndpoint();
        return endpoint?.Metadata?.GetMetadata<T>();
    }
}
