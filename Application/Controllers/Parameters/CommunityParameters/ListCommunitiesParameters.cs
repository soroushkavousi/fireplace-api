using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
}
