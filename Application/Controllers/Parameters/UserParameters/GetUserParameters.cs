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
    public class GetUserProfileInputRouteParameters : IValidator
    {
        [Required]
        [FromRoute(Name = "id-or-username")]
        public string EncodedIdOrUsername { get; set; }

        [BindNever]
        public UserIdentifier Identifier { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<UserValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            Identifier = applicationValidator.ValidateEncodedIdOrUsername(EncodedIdOrUsername);
        }
    }

    public class GetUserInputQueryParameters : IValidator
    {
        [FromQuery(Name = "include_email")]
        public bool IncludeEmail { get; set; } = true;
        [FromQuery(Name = "include_sessions")]
        public bool IncludeSessions { get; set; } = false;

        public void Validate(IServiceProvider serviceProvider)
        {

        }
    }
}
