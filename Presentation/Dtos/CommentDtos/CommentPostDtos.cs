using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Extensions;
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

namespace FireplaceApi.Presentation.Dtos;

public class ReplyToPostInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string PostEncodedId { get; set; }

    [BindNever]
    public ulong PostId { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        PostId = presentationValidator.ValidateEncodedIdFormat(PostEncodedId, FieldName.POST_ID).Value;
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class ReplyToPostInputBodyDto : IValidator
{
    [Required]
    public string Content { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(Content, FieldName.COMMENT_CONTENT);
        applicationValidator.ValidateCommentContentFormat(Content);
    }
}


public class ReplyToCommentInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string ParentCommentEncodedId { get; set; }

    [BindNever]
    public ulong ParentCommentId { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        ParentCommentId = presentationValidator.ValidateEncodedIdFormat(ParentCommentEncodedId, FieldName.COMMENT_ID).Value;
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class ReplyToCommentInputBodyDto : IValidator
{
    [Required]
    public string Content { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(Content, FieldName.COMMENT_CONTENT);
        applicationValidator.ValidateCommentContentFormat(Content);
    }
}

public class VoteCommentInputRouteDto : IValidator
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
public class VoteCommentInputBodyDto : IValidator
{
    [Required]
    public bool? IsUpvote { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(IsUpvote).ToSnakeCase()] = new OpenApiBoolean(true),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(IsUpvote, FieldName.IS_UPVOTE);
    }
}
