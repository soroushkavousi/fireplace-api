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
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class SendResetPasswordCodeInputBodyParameters : IValidator
    {
        [Required]
        public string EmailAddress { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureExample1[nameof(EmailDto.Address).ToSnakeCase()],
        };

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<UserValidator>();
            var domainValidator = applicationValidator.DomainValidator;
            var emailValidator = serviceProvider.GetService<Domain.Validators.EmailValidator>();

            applicationValidator.ValidateFieldIsNotMissing(EmailAddress, FieldName.EMAIL_ADDRESS);
            emailValidator.ValidateEmailAddressFormat(EmailAddress);
        }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ResetPasswordWithCodeInputBodyParameters : IValidator
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
            var applicationValidator = serviceProvider.GetService<UserValidator>();
            var domainValidator = applicationValidator.DomainValidator;
            var emailValidator = serviceProvider.GetService<Domain.Validators.EmailValidator>();

            applicationValidator.ValidateFieldIsNotMissing(EmailAddress, FieldName.EMAIL_ADDRESS);
            applicationValidator.ValidateFieldIsNotMissing(ResetPasswordCode, FieldName.RESET_PASSWORD_CODE);
            applicationValidator.ValidateFieldIsNotMissing(NewPassword, FieldName.NEW_PASSWORD);
            emailValidator.ValidateEmailAddressFormat(EmailAddress);
            domainValidator.ValidateResetPasswordCodeFormat(ResetPasswordCode);
            PasswordValueObject = domainValidator.ValidatePasswordFormat(NewPassword, FieldName.NEW_PASSWORD);
        }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PatchUserInputBodyParameters : IValidator
    {
        public string DisplayName { get; set; }
        public string About { get; set; }
        public string AvatarUrl { get; set; }
        public string BannerUrl { get; set; }
        public string Username { get; set; }
        [Sensitive]
        public string Password { get; set; }
        [Sensitive]
        public string NewPassword { get; set; }
        public string EmailAddress { get; set; }

        [BindNever]
        public Password OldPasswordValueObject { get; set; }
        [BindNever]
        public Password PasswordValueObject { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(DisplayName).ToSnakeCase()] = new OpenApiString("NewDisplayName"),
            [nameof(About).ToSnakeCase()] = new OpenApiString("NewAbout"),
            [nameof(AvatarUrl).ToSnakeCase()] = new OpenApiString("NewAvatarUrl"),
            [nameof(BannerUrl).ToSnakeCase()] = new OpenApiString("NewBannerUrl"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("NewUsername"),
            [nameof(Password).ToSnakeCase()] = new OpenApiString("Password"),
            [nameof(NewPassword).ToSnakeCase()] = new OpenApiString("NewPassword"),
            [nameof(EmailAddress).ToSnakeCase()] = new OpenApiString("NewEmailAddress"),
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

            if (EmailAddress != null)
                emailValidator.ValidateEmailAddressFormat(EmailAddress);

            if (Password != null && NewPassword == null)
            {
                applicationValidator.ValidateFieldIsNotMissing(NewPassword, FieldName.NEW_PASSWORD);
            }
            else if (Password == null && NewPassword != null)
            {
                applicationValidator.ValidateFieldIsNotMissing(Password, FieldName.PASSWORD);
            }
            else if (Password != null && NewPassword != null)
            {
                domainValidator.ValidatePasswordFormat(Password);
                domainValidator.ValidatePasswordFormat(NewPassword, FieldName.NEW_PASSWORD);
            }
        }
    }
}
