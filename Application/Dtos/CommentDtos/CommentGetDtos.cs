using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Dtos;

public class ListPostCommentsInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string PostEncodedId { get; set; }

    [BindNever]
    public ulong PostId { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommentValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        PostId = applicationValidator.ValidateEncodedIdFormat(PostEncodedId, FieldName.POST_ID).Value;
    }
}

public class ListPostCommentsInputQueryDto : IValidator
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(SortType))]
    public string SortString { get; set; }

    [BindNever]
    public SortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommentValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);
    }
}

public class ListCommentsInputQueryDto : IValidator
{
    [FromQuery(Name = "ids")]
    public string EncodedIds { get; set; }

    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(SortType))]
    public string SortString { get; set; }

    [BindNever]
    public List<ulong> Ids { get; private set; }
    [BindNever]
    public SortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommentValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        Ids = applicationValidator.ValidateIdsFormat(EncodedIds);
        Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);
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
        var applicationValidator = serviceProvider.GetService<CommentValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        ParentId = applicationValidator.ValidateEncodedIdFormat(ParentEncodedId, FieldName.COMMENT_ID).Value;
    }
}

public class ListChildCommentsInputQueryDto : IValidator
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(SortType))]
    public string SortString { get; set; }

    [BindNever]
    public SortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommentValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);
    }
}

public class ListSelfCommentsInputQueryDto : IValidator
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(SortType))]
    public string SortString { get; set; }

    [BindNever]
    public SortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommentValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);
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
        var applicationValidator = serviceProvider.GetService<CommentValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        Id = applicationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.COMMENT_ID).Value;
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
