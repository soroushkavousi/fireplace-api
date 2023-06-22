using FireplaceApi.Domain.Communities;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class DeleteCommunityByEncodedIdOrNameInputRouteDto : IValidator
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
