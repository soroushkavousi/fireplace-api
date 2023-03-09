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
    public class CreateCommunityMembershipInputRouteParameters : IValidator
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
}
