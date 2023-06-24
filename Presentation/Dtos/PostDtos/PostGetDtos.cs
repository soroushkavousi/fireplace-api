using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Posts;
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

public class ListCommunityPostsInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id-or-name")]
    public string CommunityEncodedIdOrName { get; set; }

    [BindNever]
    public CommunityIdentifier CommunityIdentifier { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommunityValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        CommunityIdentifier = presentationValidator.ValidateEncodedIdOrName(CommunityEncodedIdOrName);
    }
}

public class ListCommunityPostsInputQueryDto : IValidator
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(PostSortType))]
    public string SortString { get; set; }

    [BindNever]
    public PostSortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<PostValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Sort = presentationValidator.ValidateInputEnum<PostSortType>(SortString);
    }
}

public class ListPostsInputQueryDto : IValidator
{
    [FromQuery(Name = "search")]
    public string Search { get; set; }

    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(PostSortType))]
    public string SortString { get; set; }

    [FromQuery(Name = "ids")]
    public string EncodedIds { get; set; }

    [BindNever]
    public PostSortType? Sort { get; set; }
    [BindNever]
    public List<ulong> Ids { get; private set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<PostValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        if (string.IsNullOrWhiteSpace(EncodedIds))
            presentationValidator.ValidateFieldIsNotMissing(Search, FieldName.SEARCH);

        Sort = presentationValidator.ValidateInputEnum<PostSortType>(SortString);

        Ids = presentationValidator.ValidateIdsFormat(EncodedIds);
    }
}

public class ListSelfPostsInputQueryDto : IValidator
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(PostSortType))]
    public string SortString { get; set; }

    [BindNever]
    public PostSortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<PostValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Sort = presentationValidator.ValidateInputEnum<PostSortType>(SortString);
    }
}

public class GetPostByIdInputRouteDto : IValidator
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

public class GetPostByIdInputQueryDto : IValidator
{
    [FromQuery(Name = "include_author")]
    public bool IncludeAuthor { get; set; } = false;

    [FromQuery(Name = "include_community")]
    public bool IncludeCommunity { get; set; } = false;

    public void Validate(IServiceProvider serviceProvider)
    {

    }
}
