using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Extensions;
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

public class CreatePostInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id-or-name")]
    public string CommunityIdOrName { get; set; }

    [BindNever]
    public CommunityIdentifier CommunityIdentifier { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommunityValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        CommunityIdentifier = presentationValidator.ValidateEncodedIdOrName(CommunityIdOrName);
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class CreatePostInputBodyDto : IValidator
{
    [Required]
    public string Content { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(Content).ToSnakeCase()] = PostDto.PureExample1[nameof(PostDto.Content).ToSnakeCase()],
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<PostValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(Content, FieldName.POST_CONTENT);
        applicationValidator.ValidatePostContentFormat(Content);
    }
}

public class VotePostInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string EncodedId { get; set; }

    [BindNever]
    public ulong Id { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<PostValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Id = presentationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.POST_ID).Value;
    }
}

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class VotePostInputBodyDto : IValidator
{
    [Required]
    public bool? IsUpvote { get; set; }

    public static IOpenApiAny Example { get; } = new OpenApiObject
    {
        [nameof(IsUpvote).ToSnakeCase()] = new OpenApiBoolean(true),
    };

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<PostValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(IsUpvote, FieldName.IS_UPVOTE);
    }
}
