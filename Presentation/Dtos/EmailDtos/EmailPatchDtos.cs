using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Swagger;
using FireplaceApi.Presentation.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace FireplaceApi.Presentation.Dtos;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class PatchEmailInputBodyDto : IValidator
{
    public string NewAddress { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(NewAddress).ToSnakeCase()] = new OpenApiString("NewEmailAddress"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<EmailValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(NewAddress, FieldName.EMAIL_ADDRESS);
        applicationValidator.ValidateEmailAddressFormat(NewAddress);
    }
}
