using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Identifiers;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Tools;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FireplaceApi.Presentation.Dtos;

public class PatchCommunityByEncodedIdOrNameInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id-or-name")]
    public string EncodedIdOrName { get; set; }

    [BindNever]
    public CommunityIdentifier Identifier { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommunityValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Identifier = presentationValidator.ValidateEncodedIdOrName(EncodedIdOrName);
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
        var presentationValidator = serviceProvider.GetService<CommunityValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(NewName, field: FieldName.COMMUNITY_NAME);
        applicationValidator.ValidateCommunityNameFormat(NewName);
    }
}
