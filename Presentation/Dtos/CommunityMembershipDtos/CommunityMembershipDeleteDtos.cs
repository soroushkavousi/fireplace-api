using FireplaceApi.Application.Identifiers;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class DeleteCommunityMembershipByCommunityIdentifierInputRouteDto : IValidator
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
