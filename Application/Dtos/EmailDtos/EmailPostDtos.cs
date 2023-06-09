using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Dtos;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class ActivateRequestingUserEmailInputBodyDto : IValidator
{
    [Required]
    public int? ActivationCode { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(ActivationCode).ToSnakeCase()] = new OpenApiLong(11111),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<EmailValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        applicationValidator.ValidateFieldIsNotMissing(ActivationCode, FieldName.ACTIVATION_CODE);
        domainValidator.ValidateActivateCodeFormat(ActivationCode.Value);
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class ResendActivationCodeAsyncInputBodyDto : IValidator
{
    public static IOpenApiAny Example { get; } = new OpenApiObject
    {

    };
    public void Validate(IServiceProvider serviceProvider)
    {

    }
}
