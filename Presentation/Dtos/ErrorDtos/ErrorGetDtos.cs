using FireplaceApi.Application.Identifiers;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class GetErrorByCodeInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "code")]
    public int Code { get; set; }

    [BindNever]
    public ErrorIdentifier Identifier { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<ErrorValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        applicationValidator.ValidateErrorCodeFormat(Code);
        Identifier = ErrorIdentifier.OfCode(Code);
    }
}
