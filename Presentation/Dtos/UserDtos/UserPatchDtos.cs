using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Swagger;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace FireplaceApi.Presentation.Dtos;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class PatchUserInputBodyDto : IValidator
{
    public string DisplayName { get; set; }
    public string About { get; set; }
    public string AvatarUrl { get; set; }
    public string BannerUrl { get; set; }
    public Username Username { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(DisplayName).ToSnakeCase()] = new OpenApiString("NewDisplayName"),
        [nameof(About).ToSnakeCase()] = new OpenApiString("NewAbout"),
        [nameof(AvatarUrl).ToSnakeCase()] = new OpenApiString("NewAvatarUrl"),
        [nameof(BannerUrl).ToSnakeCase()] = new OpenApiString("NewBannerUrl"),
        [nameof(Username).ToSnakeCase()] = new OpenApiString("NewUsername"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;
        var emailValidator = serviceProvider.GetService<Application.Emails.EmailValidator>();

        if (DisplayName != null)
            applicationValidator.ValidateDisplayNameFormat(DisplayName);

        if (About != null)
            applicationValidator.ValidateAboutFormat(About);

        if (AvatarUrl != null)
            applicationValidator.ValidateUrlStringFormat(AvatarUrl, FieldName.AVATART_URL);

        if (BannerUrl != null)
            applicationValidator.ValidateUrlStringFormat(BannerUrl, FieldName.BANNER_URL);
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class PatchRequestingUserPasswordInputBodyDto : IValidator
{
    [Sensitive]
    public string Password { get; set; }
    [Sensitive]
    public string NewPassword { get; set; }

    [BindNever]
    public Password PasswordValueObject { get; set; }
    [BindNever]
    public Password NewPasswordValueObject { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Password).ToSnakeCase()] = new OpenApiString("Password"),
        [nameof(NewPassword).ToSnakeCase()] = new OpenApiString("NewPassword"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<UserValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;
        presentationValidator.ValidateFieldIsNotMissing(Password, FieldName.PASSWORD);
        presentationValidator.ValidateFieldIsNotMissing(NewPassword, FieldName.NEW_PASSWORD);
        PasswordValueObject = applicationValidator.ValidatePasswordFormat(Password);
        NewPasswordValueObject = applicationValidator.ValidatePasswordFormat(NewPassword, FieldName.NEW_PASSWORD);
    }
}
