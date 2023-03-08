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
    public class CreateRequestingUserPasswordInputBodyParameters : IValidator
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
            var applicationValidator = serviceProvider.GetService<UserValidator>();
            var domainValidator = applicationValidator.DomainValidator;
            applicationValidator.ValidateFieldIsNotMissing(Password, FieldName.PASSWORD);
            PasswordValueObject = domainValidator.ValidatePasswordFormat(Password);
        }
    }

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
}
