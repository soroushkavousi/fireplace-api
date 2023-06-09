using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Identifiers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FireplaceApi.Application.Dtos;

public class PatchCommunityByEncodedIdOrNameInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id-or-name")]
    public string EncodedIdOrName { get; set; }

    [BindNever]
    public CommunityIdentifier Identifier { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommunityValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        Identifier = applicationValidator.ValidateEncodedIdOrName(EncodedIdOrName);
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class PatchCommunityInputBodyDto : IValidator
{
    [JsonPropertyName("name")]
    public string NewName { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        ["name"] = new OpenApiString("new-name"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommunityValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        applicationValidator.ValidateFieldIsNotMissing(NewName, field: FieldName.COMMUNITY_NAME);
        domainValidator.ValidateCommunityNameFormat(NewName);
    }
}
