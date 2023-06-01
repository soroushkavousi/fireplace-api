using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers;

public class DeleteCommentByIdInputRouteParameters : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string EncodedId { get; set; }

    [BindNever]
    public ulong Id { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommentValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        Id = applicationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.COMMENT_ID).Value;
    }
}

public class DeleteVoteForCommentInputRouteParameters : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string EncodedId { get; set; }

    [BindNever]
    public ulong Id { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var applicationValidator = serviceProvider.GetService<CommentValidator>();
        var domainValidator = applicationValidator.DomainValidator;

        Id = applicationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.COMMENT_ID).Value;
    }
}
