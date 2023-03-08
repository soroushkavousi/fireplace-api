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
    public class PatchCommentByIdInputRouteParameters : IValidator
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
    public class PatchCommentInputBodyParameters : IValidator
    {
        public string Content { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(Content).ToSnakeCase()] = new OpenApiString("New Content"),
        };

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            applicationValidator.ValidateFieldIsNotMissing(Content, FieldName.COMMENT_CONTENT);
            domainValidator.ValidateCommentContentFormat(Content);
        }
    }
}
