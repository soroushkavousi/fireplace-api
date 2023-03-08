using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Infrastructure.Converters
{
    public class CommentConverter
    {
        private readonly ILogger<CommentConverter> _logger;
        private readonly UserConverter _authorConverter;
        private readonly PostConverter _postConverter;

        public CommentConverter(ILogger<CommentConverter> logger,
            UserConverter authorConverter, PostConverter postConverter)
        {
            _logger = logger;
            _authorConverter = authorConverter;
            _postConverter = postConverter;
        }

        // Entity

        public CommentEntity ConvertToEntity(Comment comment)
        {
            if (comment == null)
                return null;

            UserEntity authorEntity = null;
            if (comment.Author != null)
                authorEntity = _authorConverter
                    .ConvertToEntity(comment.Author.PureCopy());

            PostEntity postEntity = null;
            if (comment.Post != null)
                postEntity = _postConverter
                    .ConvertToEntity(comment.Post.PureCopy());

            CommentEntity parentCommentEntity = null;
            if (comment.ParentComment != null)
                parentCommentEntity =
                    ConvertToEntity(comment.ParentComment.PureCopy());

            List<CommentEntity> childCommentEntities = null;
            if (comment.ChildComments != null)
                childCommentEntities = comment.ChildComments
                    .Select(cc => ConvertToEntity(cc.PureCopy())).ToList();

            var commentEntity = new CommentEntity(
                comment.Id, comment.AuthorId, comment.AuthorUsername,
                comment.PostId, comment.Content, comment.ParentCommentId,
                comment.Vote, comment.RequestingUserVote,
                comment.CreationDate, comment.ModifiedDate,
                authorEntity, postEntity, parentCommentEntity,
                childCommentEntities);

            return commentEntity;
        }

        public Comment ConvertToModel(CommentEntity commentEntity)
        {
            if (commentEntity == null)
                return null;

            User author = null;
            if (commentEntity.AuthorEntity != null)
                author = _authorConverter.ConvertToModel(commentEntity.AuthorEntity.PureCopy());

            Post post = null;
            if (commentEntity.PostEntity != null)
                post = _postConverter
                    .ConvertToModel(commentEntity.PostEntity.PureCopy());

            Comment parentComment = null;
            if (commentEntity.ParentCommentEntity != null)
                parentComment = ConvertToModel(
                    commentEntity.ParentCommentEntity.PureCopy());

            List<Comment> childComments = null;
            if (commentEntity.ChildCommentEntities != null)
                childComments = commentEntity.ChildCommentEntities
                    .Select(cc => ConvertToModel(cc.PureCopy())).ToList();

            var comment = new Comment(commentEntity.Id,
                commentEntity.AuthorEntityId, commentEntity.AuthorEntityUsername,
                commentEntity.PostEntityId, commentEntity.Vote,
                commentEntity.RequestingUserVote, commentEntity.Content,
                commentEntity.CreationDate, commentEntity.ParentCommentEntityId,
                commentEntity.ModifiedDate, author, post, parentComment, childComments);

            return comment;
        }
    }
}
