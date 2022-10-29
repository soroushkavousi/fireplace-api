using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
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
            if (!comment.ChildComments.IsNullOrEmpty())
            {
                childCommentDtos = new List<CommentDto>();
                foreach (var childComment in comment.ChildComments)
                {
                    var childCommentDto = ConvertToDto(
                        childComment.PureCopy());
                    childCommentDtos.Add(childCommentDto);
                }
            }

            List<string> moreChildCommentEncodedIds = null;
            if (!comment.MoreChildCommentIds.IsNullOrEmpty())
            {
                moreChildCommentEncodedIds = new List<string>();
                foreach (var childCommentId in comment.MoreChildCommentIds)
                {
                    moreChildCommentEncodedIds.Add(childCommentId.IdEncode());
                }
            }

            var commentDto = new CommentDto(comment.Id.IdEncode(),
                comment.AuthorId.IdEncode(), comment.AuthorUsername,
                comment.PostId.IdEncode(), comment.Vote,
                comment.RequestingUserVote,
                comment.Content, comment.CreationDate,
                comment.ParentCommentId.IdEncode(),
                comment.ModifiedDate, authorDto, postDto,
                childCommentDtos, moreChildCommentEncodedIds);

            return commentDto;
        }
    }
}
