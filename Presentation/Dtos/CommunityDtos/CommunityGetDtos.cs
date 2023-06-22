using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Identifiers;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Tools;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class ListCommunitiesInputQueryDto : IValidator
{
    [FromQuery(Name = "search")]
    public string Search { get; set; }

    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(CommunitySortType))]
    public string SortString { get; set; }

    [FromQuery(Name = "ids")]
    public string EncodedIds { get; set; }


    [BindNever]
    public CommunitySortType? Sort { get; set; }
    [BindNever]
    public List<ulong> Ids { get; private set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommunityValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        if (string.IsNullOrWhiteSpace(EncodedIds))
            presentationValidator.ValidateFieldIsNotMissing(Search, FieldName.SEARCH);

        Sort = presentationValidator.ValidateInputEnum<CommunitySortType>(SortString);

        Ids = presentationValidator.ValidateIdsFormat(EncodedIds);
    }
}

public class ListJoinedCommunitiesInputQueryDto : IValidator
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(CommunitySortType))]
    public string SortString { get; set; }

    [BindNever]
    public CommunitySortType? Sort { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommunityValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Sort = presentationValidator.ValidateInputEnum<CommunitySortType>(SortString);
    }
}

public class GetCommunityByIdOrNameInputRouteDto : IValidator
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
