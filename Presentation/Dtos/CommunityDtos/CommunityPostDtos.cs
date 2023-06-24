using FireplaceApi.Domain.Errors;
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
public class CreateCommunityInputBodyDto : IValidator
{
    [Required]
    public string Name { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Name).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommunityValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(Name, FieldName.COMMUNITY_NAME);
        applicationValidator.ValidateCommunityNameFormat(Name);
    }
}
