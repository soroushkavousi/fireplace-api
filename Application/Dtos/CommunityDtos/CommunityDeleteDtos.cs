using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Identifiers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Dtos;

public class DeleteCommunityByEncodedIdOrNameInputRouteDto : IValidator
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
