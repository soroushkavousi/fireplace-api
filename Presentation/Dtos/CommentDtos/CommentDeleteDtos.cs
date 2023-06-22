using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Interfaces;
using FireplaceApi.Presentation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class DeleteCommentByIdInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string EncodedId { get; set; }

    [BindNever]
    public ulong Id { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Id = presentationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.COMMENT_ID).Value;
    }
}

public class DeleteVoteForCommentInputRouteDto : IValidator
{
    [Required]
    [FromRoute(Name = "id")]
    public string EncodedId { get; set; }

    [BindNever]
    public ulong Id { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<CommentValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        Id = presentationValidator.ValidateEncodedIdFormat(EncodedId, FieldName.COMMENT_ID).Value;
    }
}
