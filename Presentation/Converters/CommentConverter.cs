using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Common;
using FireplaceApi.Presentation.Dtos;
using System.Collections.Generic;

namespace FireplaceApi.Presentation.Converters;

public static class CommentConverter
{
    public static CommentDto ToDto(this Comment comment)
    {
        if (comment == null)
            return null;

        ProfileDto authorDto = null;
        if (comment.Author != null)
            authorDto = comment.Author.PureCopy().ToProfileDto();

        PostDto postDto = null;
        if (comment.Post != null)
            postDto = comment.Post.PureCopy().ToDto();

        List<CommentDto> childCommentDtos = null;
        if (!comment.ChildComments.IsNullOrEmpty())
        {
            childCommentDtos = new List<CommentDto>();
            foreach (var childComment in comment.ChildComments)
            {
                var childCommentDto = childComment.PureCopy().ToDto();
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

    public static QueryResultDto<CommentDto> ToDto(this QueryResult<Comment> queryResult)
        => queryResult.ToDto(ToDto);
}
