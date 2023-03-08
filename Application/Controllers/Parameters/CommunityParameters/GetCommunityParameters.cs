using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Identifiers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
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

    public class GetCommunityInputQueryParameters : IValidator
    {
        [FromQuery(Name = "include_creator")]
        public bool IncludeCreator { get; set; } = true;

        public void Validate(IServiceProvider serviceProvider)
        {

        }
    }
}
