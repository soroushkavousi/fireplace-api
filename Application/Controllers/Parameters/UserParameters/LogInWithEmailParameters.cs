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
using System.Net;

namespace FireplaceApi.Application.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class LogInWithEmailInputBodyParameters : IValidator
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
            var applicationValidator = serviceProvider.GetService<UserValidator>();
            var domainValidator = applicationValidator.DomainValidator;
            var emailValidator = serviceProvider.GetService<Domain.Validators.EmailValidator>();

            applicationValidator.ValidateFieldIsNotMissing(EmailAddress, FieldName.EMAIL_ADDRESS);
            applicationValidator.ValidateFieldIsNotMissing(Password, FieldName.PASSWORD);
            emailValidator.ValidateEmailAddressFormat(EmailAddress);
            PasswordValueObject = domainValidator.ValidatePasswordFormat(Password);
        }
    }

    public class LogInWithEmailOutputCookieParameters : IOutputCookieParameters
    {
        [Required]
        [Sensitive]
        public string AccessToken { get; set; }

        public LogInWithEmailOutputCookieParameters(string accessToken)
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
}
