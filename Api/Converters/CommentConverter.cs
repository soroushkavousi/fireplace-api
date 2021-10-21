using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Api.Converters
{
    public class CommentConverter
    {
        private readonly ILogger<CommentConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommentConverter(ILogger<CommentConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public CommentDto ConvertToDto(Comment comment)
        {
            if (comment == null)
                return null;

            UserDto authorDto = null;
            if (comment.Author != null)
                authorDto = _serviceProvider.GetService<UserConverter>()
                    .ConvertToDto(comment.Author.PureCopy());

            PostDto postDto = null;
            if (comment.Post != null)
                postDto = _serviceProvider.GetService<PostConverter>()
                    .ConvertToDto(comment.Post.PureCopy());

            List<CommentDto> childCommentDtos = null;
            if (comment.ChildComments != null &&
                comment.ChildComments.Count != 0)
            {
                childCommentDtos = new List<CommentDto>();
                foreach (var childComment in comment.ChildComments)
                {
                    var childCommentDto = ConvertToDto(
                        childComment.PureCopy());
                    childCommentDtos.Add(childCommentDto);
                }
            }

            var commentDto = new CommentDto(comment.Id,
                comment.AuthorId, comment.AuthorUsername,
                comment.PostId, comment.Vote, comment.Content, comment.CreationDate,
                comment.ModifiedDate, comment.ParentCommentIds, authorDto, postDto,
                childComments: childCommentDtos);

            return commentDto;
        }

        public PageDto<CommentDto> ConvertToDto(Page<Comment> page, string pureRequestPath)
        {
            if (page == null)
                return null;

            var listPath = $"{GlobalOperator.GlobalValues.Api.BaseUrlPath}{pureRequestPath}";

            var paginationDto = new PaginationDto(page.QueryResultPointer,
                pureRequestPath, page.Number, page.Start, page.End, page.Limit,
                page.TotalItemsCount, page.TotalPagesCount);

            var itemDtos = page.Items.Select(comment => ConvertToDto(comment)).ToList();

            var pageDto = new PageDto<CommentDto>(itemDtos, paginationDto);
            return pageDto;
        }
    }
}
