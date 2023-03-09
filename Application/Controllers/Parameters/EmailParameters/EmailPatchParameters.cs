using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace FireplaceApi.Application.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PatchEmailInputBodyParameters : IValidator
    {
        public string NewAddress { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(NewAddress).ToSnakeCase()] = new OpenApiString("NewEmailAddress"),
        };

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<EmailValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            applicationValidator.ValidateFieldIsNotMissing(NewAddress, FieldName.EMAIL_ADDRESS);
            domainValidator.ValidateEmailAddressFormat(NewAddress);
        }
    }
}
