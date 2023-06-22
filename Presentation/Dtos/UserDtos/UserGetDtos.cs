using FireplaceApi.Application.Attributes;
using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Identifiers;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Tools;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FireplaceApi.Presentation.Dtos;

public class GetUserProfileInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id-or-username")]
    public string EncodedIdOrUsername { get; set; }

    [BindNever]
    public UserIdentifier Identifier { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Identifier = presentationValidator.ValidateEncodedIdOrUsername(EncodedIdOrUsername);
    }
}

public class GetUserInputQueryDto : IValidator
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
public class LogInWithGoogleInputQueryDto : IValidator
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
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        // TODO
        presentationValidator.ValidateFieldIsNotMissing(Code, FieldName.GOOGLE_CODE);
    }
}

public class LogInWithGoogleOutputCookieDto : IOutputCookieDto
{
    [Required]
    [Sensitive]
    public string AccessToken { get; set; }

    public LogInWithGoogleOutputCookieDto(string accessToken)
    {
        AccessToken = accessToken;
    }

    public CookieCollection GetCookieCollection()
    {
        var cookieCollection = new CookieCollection
        {
            new Cookie(AuthConstants.AccessTokenCookieKey, AccessToken)
        };
        return cookieCollection;
    }
}
