using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class RevokeSessionInputRouteDto
{
    [Required]
    [FromRoute(Name = "id")]
    public string EncodedId { get; set; }

    [BindNever]
    public ulong Id { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<SessionValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Id = presentationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.POST_ID).Value;
    }
}
