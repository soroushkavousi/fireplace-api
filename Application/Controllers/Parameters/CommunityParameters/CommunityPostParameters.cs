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

namespace FireplaceApi.Application.Controllers;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class CreateCommunityInputBodyParameters : IValidator
{
    [Required]
    public string Name { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Name).ToSnakeCase()] = CommunityDto.PureExample1[nameof(CommunityDto.Name).ToSnakeCase()],
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommunityValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        applicationValidator.ValidateFieldIsNotMissing(Name, FieldName.COMMUNITY_NAME);
        domainValidator.ValidateCommunityNameFormat(Name);
    }
}
