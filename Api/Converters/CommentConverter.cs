using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Api.Converters
{
    public class CommentConverter : BaseConverter<Comment, CommentDto>
    {
        private readonly ILogger<CommentConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommentConverter(ILogger<CommentConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override CommentDto ConvertToDto(Comment comment)
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
    }
}
