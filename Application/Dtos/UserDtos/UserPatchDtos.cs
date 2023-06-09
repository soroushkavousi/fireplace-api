using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Attributes;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace FireplaceApi.Application.Dtos;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class PatchUserInputBodyDto : IValidator
{
    public string DisplayName { get; set; }
    public string About { get; set; }
    public string AvatarUrl { get; set; }
    public string BannerUrl { get; set; }
    public string Username { get; set; }

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
        var applicationValidator = serviceProvider.GetService<UserValidator>();
        var domainValidator = applicationValidator.DomainValidator;
        var emailValidator = serviceProvider.GetService<Domain.Validators.EmailValidator>();

        if (DisplayName != null)
            domainValidator.ValidateDisplayNameFormat(DisplayName);

        if (About != null)
            domainValidator.ValidateAboutFormat(About);

        if (AvatarUrl != null)
            domainValidator.ValidateUrlStringFormat(AvatarUrl, FieldName.AVATART_URL);

        if (BannerUrl != null)
            domainValidator.ValidateUrlStringFormat(BannerUrl, FieldName.BANNER_URL);

        if (Username != null)
            domainValidator.ValidateUsernameFormat(Username);
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
        var applicationValidator = serviceProvider.GetService<UserValidator>();
        var domainValidator = applicationValidator.DomainValidator;
        applicationValidator.ValidateFieldIsNotMissing(Password, FieldName.PASSWORD);
        applicationValidator.ValidateFieldIsNotMissing(NewPassword, FieldName.NEW_PASSWORD);
        PasswordValueObject = domainValidator.ValidatePasswordFormat(Password);
        NewPasswordValueObject = domainValidator.ValidatePasswordFormat(NewPassword, FieldName.NEW_PASSWORD);
    }
}
