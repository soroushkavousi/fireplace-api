using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Swagger;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class PatchCommentByIdInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string EncodedId { get; set; }

    [BindNever]
    public ulong Id { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Id = presentationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.COMMENT_ID).Value;
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class PatchCommentInputBodyDto : IValidator
{
    public string Content { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Content).ToSnakeCase()] = new OpenApiString("New Content"),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(Content, FieldName.COMMENT_CONTENT);
        applicationValidator.ValidateCommentContentFormat(Content);
    }
}

public class ToggleVoteForCommentInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string EncodedId { get; set; }

    [BindNever]
    public ulong Id { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Id = presentationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.COMMENT_ID).Value;
    }
}
