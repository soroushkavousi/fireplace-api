using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Swagger;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace FireplaceApi.Presentation.Dtos;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class SignUpWithEmailInputBodyDto : IValidator
{
    [Required]
    public string EmailAddress { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    [Sensitive]
    public string Password { get; set; }

    [BindNever, JsonIgnore]
    public Password PasswordValueObject { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureExample1[nameof(EmailDto.Address).ToSnakeCase()],
        [nameof(Username).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
        [nameof(Password).ToSnakeCase()] = new OpenApiString("P@ssw0rd"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;
        var emailValidator = serviceProvider.GetService<Application.Emails.EmailValidator>();

        presentationValidator.ValidateFieldIsNotMissing(EmailAddress, FieldName.EMAIL_ADDRESS);
        presentationValidator.ValidateFieldIsNotMissing(Password, FieldName.PASSWORD);
        emailValidator.ValidateEmailAddressFormat(EmailAddress);
        PasswordValueObject = applicationValidator.ValidatePasswordFormat(Password);
    }
}

public class SignUpWithEmailOutputCookieDto : IOutputCookieDto
{
    [Required]
    [Sensitive]
    public string AccessToken { get; set; }

    public SignUpWithEmailOutputCookieDto(string accessToken)
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

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class LogInWithEmailInputBodyDto : IValidator
{
    [Required]
    public string EmailAddress { get; set; }
    [Required]
    [Sensitive]
    public string Password { get; set; }

    [BindNever]
    public Password PasswordValueObject { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureExample1[nameof(EmailDto.Address).ToSnakeCase()],
        [nameof(Password).ToSnakeCase()] = new OpenApiString("P@ssw0rd"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;
        var emailValidator = serviceProvider.GetService<Application.Emails.EmailValidator>();

        presentationValidator.ValidateFieldIsNotMissing(EmailAddress, FieldName.EMAIL_ADDRESS);
        presentationValidator.ValidateFieldIsNotMissing(Password, FieldName.PASSWORD);
        emailValidator.ValidateEmailAddressFormat(EmailAddress);
        PasswordValueObject = applicationValidator.ValidatePasswordFormat(Password);
    }
}

public class LogInWithEmailOutputCookieDto : IOutputCookieDto
{
    [Required]
    [Sensitive]
    public string AccessToken { get; set; }

    public LogInWithEmailOutputCookieDto(string accessToken)
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

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class LogInWithUsernameInputBodyDto : IValidator
{
    [Required]
    public string Username { get; set; }
    [Required]
    [Sensitive]
    public string Password { get; set; }

    [BindNever, JsonIgnore]
    public Password PasswordValueObject { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Username).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Username).ToSnakeCase()],
        [nameof(Password).ToSnakeCase()] = new OpenApiString("P@ssw0rd"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(Password, FieldName.PASSWORD);
        PasswordValueObject = applicationValidator.ValidatePasswordFormat(Password);
    }
}

public class LogInWithUsernameOutputCookieDto : IOutputCookieDto
{
    [Required]
    [Sensitive]
    public string AccessToken { get; set; }

    public LogInWithUsernameOutputCookieDto(string accessToken)
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

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class CreateRequestingUserPasswordInputBodyDto : IValidator
{
    [Sensitive]
    public string Password { get; set; }

    [BindNever]
    public Password PasswordValueObject { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Password).ToSnakeCase()] = new OpenApiString("Password"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;
        presentationValidator.ValidateFieldIsNotMissing(Password, FieldName.PASSWORD);
        PasswordValueObject = applicationValidator.ValidatePasswordFormat(Password);
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class SendResetPasswordCodeInputBodyDto : IValidator
{
    [Required]
    public string EmailAddress { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureExample1[nameof(EmailDto.Address).ToSnakeCase()],
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;
        var emailValidator = serviceProvider.GetService<Application.Emails.EmailValidator>();

        presentationValidator.ValidateFieldIsNotMissing(EmailAddress, FieldName.EMAIL_ADDRESS);
        emailValidator.ValidateEmailAddressFormat(EmailAddress);
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class ResetPasswordWithCodeInputBodyDto : IValidator
{
    [Required]
    public string EmailAddress { get; set; }
    [Required]
    public string ResetPasswordCode { get; set; }
    [Required]
    public string NewPassword { get; set; }

    [BindNever]
    public Password PasswordValueObject { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureExample1[nameof(EmailDto.Address).ToSnakeCase()],
        [nameof(ResetPasswordCode).ToSnakeCase()] = new OpenApiString("6Qw2RsG8aw"),
        [nameof(NewPassword).ToSnakeCase()] = new OpenApiString("NewPassword@123"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;
        var emailValidator = serviceProvider.GetService<Application.Emails.EmailValidator>();

        presentationValidator.ValidateFieldIsNotMissing(EmailAddress, FieldName.EMAIL_ADDRESS);
        presentationValidator.ValidateFieldIsNotMissing(ResetPasswordCode, FieldName.RESET_PASSWORD_CODE);
        presentationValidator.ValidateFieldIsNotMissing(NewPassword, FieldName.NEW_PASSWORD);
        emailValidator.ValidateEmailAddressFormat(EmailAddress);
        applicationValidator.ValidateResetPasswordCodeFormat(ResetPasswordCode);
        PasswordValueObject = applicationValidator.ValidatePasswordFormat(NewPassword, FieldName.NEW_PASSWORD);
    }
}
