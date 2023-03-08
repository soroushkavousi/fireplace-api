using FireplaceApi.Application.Interfaces;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    public class ListPostCommentsInputRouteParameters : IValidator
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

    public class ListPostCommentsInputQueryParameters : IValidator
    {
        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string SortString { get; set; }

        [BindNever]
        public SortType? Sort { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);
        }
    }

    public class ListCommentsInputQueryParameters : IValidator
    {
        [FromQuery(Name = "ids")]
        public string EncodedIds { get; set; }

        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string SortString { get; set; }

        [BindNever]
        public List<ulong> Ids { get; private set; }
        [BindNever]
        public SortType? Sort { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            Ids = applicationValidator.ValidateIdsFormat(EncodedIds);
            Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);
        }
    }

    public class ListChildCommentsInputRouteParameters : IValidator
    {
        [Required]
        [FromRoute(Name = "id")]
        public string ParentEncodedId { get; set; }

        [BindNever]
        public ulong ParentId { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            ParentId = applicationValidator.ValidateEncodedIdFormat(ParentEncodedId, FieldName.COMMENT_ID).Value;
        }
    }

    public class ListSelfCommentsInputQueryParameters : IValidator
    {
        [FromQuery(Name = "sort")]
        [SwaggerEnum(Type = typeof(SortType))]
        public string SortString { get; set; }

        [BindNever]
        public SortType? Sort { get; set; }

        public void Validate(IServiceProvider serviceProvider)
        {
            var applicationValidator = serviceProvider.GetService<CommentValidator>();
            var domainValidator = applicationValidator.DomainValidator;

            Sort = applicationValidator.ValidateInputEnum<SortType>(SortString);
        }
    }
}
