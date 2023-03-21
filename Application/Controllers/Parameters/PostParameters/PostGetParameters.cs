using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Identifiers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    public class ListCommunityPostsInputRouteParameters : IValidator
    {
        [Required]
        [FromRoute(Name = "id-or-name")]
        public string CommunityEncodedIdOrName { get; set; }

        [BindNever]
        public CommunityIdentifier CommunityIdentifier { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommunityValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            CommunityIdentifier = applicationValidator.ValidateEncodedIdOrName(CommunityEncodedIdOrName);
        }
    }

    public class ListCommunityPostsInputQueryParameters : IValidator
    {
        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string SortString { get; set; }

        [BindNever]
        public SortType? Sort { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<PostValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);
        }
    }

    public class ListPostsInputQueryParameters : IValidator
    {
        [FromQuery(Name = "search")]
        public string Search { get; set; }

        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string SortString { get; set; }

        [FromQuery(Name = "ids")]
        public string EncodedIds { get; set; }

        [BindNever]
        public SortType? Sort { get; set; }
        [BindNever]
        public List<ulong> Ids { get; private set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<PostValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            if (string.IsNullOrWhiteSpace(EncodedIds))
                applicationValidator.ValidateFieldIsNotMissing(Search, FieldName.SEARCH);

            Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);

            Ids = applicationValidator.ValidateIdsFormat(EncodedIds);
        }
    }

    public class ListSelfPostsInputQueryParameters : IValidator
    {
        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string SortString { get; set; }

        [BindNever]
        public SortType? Sort { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<PostValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);
        }
    }

    public class GetPostByIdInputRouteParameters : IValidator
    {
        [Required]
        [FromRoute(Name = "id")]
        public string EncodedId { get; set; }

        [BindNever]
        public ulong Id { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<PostValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            Id = applicationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.POST_ID).Value;
        }
    }

    public class GetPostByIdInputQueryParameters : IValidator
    {
        [FromQuery(Name = "include_author")]
        public bool IncludeAuthor { get; set; } = false;

        [FromQuery(Name = "include_community")]
        public bool IncludeCommunity { get; set; } = false;

        public void Validate(IServiceProvider serviceProvider)
        {

        }
    }
}
