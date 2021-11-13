﻿using FireplaceApi.Core.Models;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters
{
    public class CommentConverter
    {
        private readonly ILogger<CommentConverter> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserConverter _authorConverter;
        private readonly PostConverter _postConverter;

        public CommentConverter(ILogger<CommentConverter> logger,
            IServiceProvider serviceProvider, UserConverter authorConverter,
            PostConverter postConverter)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
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

            var commentEntity = new CommentEntity(
                comment.Id, comment.AuthorId,
                comment.AuthorUsername, comment.PostId,
                comment.Content, comment.ParentCommentIds.ToDecimals(),
                comment.CreationDate, comment.ModifiedDate,
                comment.Vote, authorEntity, postEntity);

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

            int requestingUserVote = 0;
            if (commentEntity.CommentVoteEntities != null
                && commentEntity.CommentVoteEntities.Count == 1)
            {
                var voteEntity = commentEntity.CommentVoteEntities[0];
                if (voteEntity.IsUp)
                    requestingUserVote = 1;
                else
                    requestingUserVote = -1;
            }

            var comment = new Comment(commentEntity.Id,
                commentEntity.AuthorEntityId, commentEntity.AuthorEntityUsername,
                commentEntity.PostEntityId, commentEntity.Vote,
                requestingUserVote, commentEntity.Content,
                commentEntity.CreationDate, commentEntity.ParentCommentEntityIds.ToUlongs(),
                commentEntity.ModifiedDate, author, post);

            return comment;
        }
    }
}