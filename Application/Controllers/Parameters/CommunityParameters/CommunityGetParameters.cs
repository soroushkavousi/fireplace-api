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
    public class ListCommunitiesInputQueryParameters : IValidator
    {
        [FromQuery(Name = "name")]
        public string Name { get; set; }

        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(CommunitySortType))]
        public string SortString { get; set; }

        [FromQuery(Name = "ids")]
        public string EncodedIds { get; set; }


        [BindNever]
        public SortType? Sort { get; set; }
        [BindNever]
        public List<ulong> Ids { get; private set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommunityValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            Sort = (SortType?)applicationValidator.ValidateInputEnum<CommunitySortType>(SortString);

            Ids = applicationValidator.ValidateIdsFormat(EncodedIds);
        }
    }

    public class GetCommunityByIdOrNameInputRouteParameters : IValidator
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
}
