using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Posts;
using FireplaceApi.Domain.Users;
using FireplaceApi.Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Infrastructure.Converters;

public static class CommentConverter
{
    // Entity

    public static CommentEntity ToEntity(this Comment comment)
    {
        if (comment == null)
            return null;

        UserEntity authorEntity = null;
        if (comment.Author != null)
            authorEntity = comment.Author.PureCopy().ToEntity();

        PostEntity postEntity = null;
        if (comment.Post != null)
            postEntity = comment.Post.PureCopy().ToEntity();

        CommentEntity parentCommentEntity = null;
        if (comment.ParentComment != null)
            parentCommentEntity = comment.ParentComment.PureCopy().ToEntity();

        List<CommentEntity> childCommentEntities = null;
        if (comment.ChildComments != null)
            childCommentEntities = comment.ChildComments
                .Select(cc => cc.PureCopy().ToEntity()).ToList();

        var commentEntity = new CommentEntity(
            comment.Id, comment.AuthorId, comment.AuthorUsername,
            comment.PostId, comment.Content, comment.ParentCommentId,
            comment.Vote, comment.RequestingUserVote,
            comment.CreationDate, comment.ModifiedDate,
            authorEntity, postEntity, parentCommentEntity,
            childCommentEntities);

        return commentEntity;
    }

    public static Comment ToModel(this CommentEntity commentEntity)
    {
        if (commentEntity == null)
            return null;

        User author = null;
        if (commentEntity.AuthorEntity != null)
            author = commentEntity.AuthorEntity.PureCopy().ToModel();

        Post post = null;
        if (commentEntity.PostEntity != null)
            post = commentEntity.PostEntity.PureCopy().ToModel();

        Comment parentComment = null;
        if (commentEntity.ParentCommentEntity != null)
            parentComment = commentEntity.ParentCommentEntity.PureCopy().ToModel();

        List<Comment> childComments = null;
        if (commentEntity.ChildCommentEntities != null)
            childComments = commentEntity.ChildCommentEntities
                .Select(cc => cc.PureCopy().ToModel()).ToList();

        var comment = new Comment(commentEntity.Id,
            commentEntity.AuthorEntityId, commentEntity.AuthorEntityUsername,
            commentEntity.PostEntityId, commentEntity.Vote,
            commentEntity.RequestingUserVote, commentEntity.Content,
            commentEntity.CreationDate, commentEntity.ParentCommentEntityId,
            commentEntity.ModifiedDate, author, post, parentComment, childComments);

        return comment;
    }
}
