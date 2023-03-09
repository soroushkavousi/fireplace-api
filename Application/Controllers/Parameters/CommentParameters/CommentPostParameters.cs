using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    public class ReplyToPostInputRouteParameters : IValidator
    {
        [Required]
        [FromRoute(Name = "id")]
        public string PostEncodedId { get; set; }

        [BindNever]
        public ulong PostId { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            PostId = applicationValidator.ValidateEncodedIdFormat(PostEncodedId, FieldName.POST_ID).Value;
        }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ReplyToPostInputBodyParameters : IValidator
    {
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
        };

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            applicationValidator.ValidateFieldIsNotMissing(Content, FieldName.COMMENT_CONTENT);
            domainValidator.ValidateCommentContentFormat(Content);
        }
    }


    public class ReplyToCommentInputRouteParameters : IValidator
    {
        [Required]
        [FromRoute(Name = "id")]
        public string ParentCommentEncodedId { get; set; }

        [BindNever]
        public ulong ParentCommentId { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            ParentCommentId = applicationValidator.ValidateEncodedIdFormat(ParentCommentEncodedId, FieldName.COMMENT_ID).Value;
        }
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ReplyToCommentInputBodyParameters : IValidator
    {
        [Required]
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = CommentDto.PureExample1[nameof(CommentDto.Content).ToSnakeCase()],
        };

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            applicationValidator.ValidateFieldIsNotMissing(Content, FieldName.COMMENT_CONTENT);
            domainValidator.ValidateCommentContentFormat(Content);
        }
    }

    public class VoteCommentInputRouteParameters : IValidator
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

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class VoteCommentInputBodyParameters : IValidator
    {
        [Required]
        public bool? IsUpvote { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(IsUpvote).ToSnakeCase()] = new OpenApiBoolean(true),
        };

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            applicationValidator.ValidateFieldIsNotMissing(IsUpvote, FieldName.IS_UPVOTE);
        }
    }
}
