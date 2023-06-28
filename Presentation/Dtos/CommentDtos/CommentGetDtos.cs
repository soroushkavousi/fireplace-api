using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Swagger;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class ListPostCommentsInputRouteDto : IValidator
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

public class ListPostCommentsInputQueryDto : IValidator
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(CommentSortType))]
    public string SortString { get; set; }

    [BindNever]
    public CommentSortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Sort = SortString.ToNullableEnum<CommentSortType>();
    }
}

public class ListCommentsInputQueryDto : IValidator
{
    [FromQuery(Name = "ids")]
    public string EncodedIds { get; set; }

    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(CommentSortType))]
    public string SortString { get; set; }

    [BindNever]
    public List<ulong> Ids { get; private set; }
    [BindNever]
    public CommentSortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Ids = presentationValidator.ValidateIdsFormat(EncodedIds);
        Sort = SortString.ToNullableEnum<CommentSortType>();
    }
}

public class ListChildCommentsInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string ParentEncodedId { get; set; }

    [BindNever]
    public ulong ParentId { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        ParentId = presentationValidator.ValidateEncodedIdFormat(ParentEncodedId, FieldName.COMMENT_ID).Value;
    }
}

public class ListChildCommentsInputQueryDto : IValidator
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(CommentSortType))]
    public string SortString { get; set; }

    [BindNever]
    public CommentSortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Sort = SortString.ToNullableEnum<CommentSortType>();
    }
}

public class ListSelfCommentsInputQueryDto : IValidator
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(CommentSortType))]
    public string SortString { get; set; }

    [BindNever]
    public CommentSortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Sort = SortString.ToNullableEnum<CommentSortType>();
    }
}

public class GetCommentByIdInputRouteDto : IValidator
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

public class GetCommentInputQueryDto : IValidator
{
    [FromQuery(Name = "include_author")]
    public bool IncludeAuthor { get; set; } = false;

    [FromQuery(Name = "include_post")]
    public bool IncludePost { get; set; } = false;

    public void Validate(IServiceProvider serviceProvider)
    {

    }
}
