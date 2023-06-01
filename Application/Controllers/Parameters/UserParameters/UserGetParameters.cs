using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Attributes;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Identifiers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FireplaceApi.Application.Controllers;

public class GetUserProfileInputRouteParameters : IValidator
{
    [Required]
    [FromRoute(Name = "id-or-username")]
    public string EncodedIdOrUsername { get; set; }

    [BindNever]
    public UserIdentifier Identifier { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<UserValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        Identifier = applicationValidator.ValidateEncodedIdOrUsername(EncodedIdOrUsername);
    }
}

public class GetUserInputQueryParameters : IValidator
{
    [FromQuery(Name = "include_email")]
    public bool IncludeEmail { get; set; } = true;
    [FromQuery(Name = "include_sessions")]
    public bool IncludeSessions { get; set; } = false;

    public void Validate(IServiceProvider serviceProvider)
    {

    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class LogInWithGoogleInputQueryParameters : IValidator
{
    [Required]
    [FromQuery(Name = "state")]
    public string State { get; set; }

    [Required]
    [FromQuery(Name = "code")]
    [Sensitive]
    public string Code { get; set; }

    [Required]
    [FromQuery(Name = "scope")]
    public string Scope { get; set; }

    [Required]
    [FromQuery(Name = "authuser")]
    public string AuthUser { get; set; }

    [Required]
    [FromQuery(Name = "prompt")]
    public string Prompt { get; set; }

    [Required]
    [FromQuery(Name = "error")]
    public string Error { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(State).ToSnakeCase()] = new OpenApiString("M3VuIBEwciAyNiAyMDIwIDE1Oj30IjE0IEdNVCswDNMwIChJcmFuIERheWxpZ2h0IFRabWMp"),
        [nameof(Code).ToSnakeCase()] = new OpenApiString("4/zAH61K5JT4LkfzhmImjSjCiUFQxfhOdupfa9dnt8ao0Yy4V_kVyvevHNUr5r6RM5th0MaQEzuf5ixlFIkrCVHQ0"),
        [nameof(Scope).ToSnakeCase()] = new OpenApiString("email profile openid https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email"),
        [nameof(AuthUser).ToSnakeCase()] = new OpenApiString("12345"),
        [nameof(Prompt).ToSnakeCase()] = new OpenApiString("consent"),
        [nameof(Error).ToSnakeCase()] = new OpenApiString("access_denied"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<UserValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        // TODO
        applicationValidator.ValidateFieldIsNotMissing(Code, FieldName.GOOGLE_CODE);
    }
}

public class LogInWithGoogleOutputCookieParameters : IOutputCookieParameters
{
    [Required]
    [Sensitive]
    public string AccessToken { get; set; }

    public LogInWithGoogleOutputCookieParameters(string accessToken)
    {
        AccessToken = accessToken;
    }

    public CookieCollection GetCookieCollection()
    {
        var cookieCollection = new CookieCollection
        {
            new Cookie(Constants.ResponseAccessTokenCookieKey, AccessToken)
        };
        return cookieCollection;
    }
}
