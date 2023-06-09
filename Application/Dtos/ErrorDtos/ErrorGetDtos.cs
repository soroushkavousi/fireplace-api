using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Identifiers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Dtos;

public class GetErrorByCodeInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "code")]
    public int Code { get; set; }

    [BindNever]
    public ErrorIdentifier Identifier { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<ErrorValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        domainValidator.ValidateErrorCodeFormat(Code);
        Identifier = ErrorIdentifier.OfCode(Code);
    }
}
