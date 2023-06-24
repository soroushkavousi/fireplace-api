using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Swagger;
using FireplaceApi.Presentation.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

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
        var presentationValidator = serviceProvider.GetService<EmailValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(ActivationCode, FieldName.ACTIVATION_CODE);
        applicationValidator.ValidateActivateCodeFormat(ActivationCode.Value);
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
