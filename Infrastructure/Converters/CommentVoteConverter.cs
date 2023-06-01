using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters;

public class CommentVoteConverter
{
    private readonly ILogger<CommentVoteConverter> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly UserConverter _voterConverter;
    private readonly CommentConverter _commentConverter;

    public CommentVoteConverter(ILogger<CommentVoteConverter> logger,
        IServiceProvider serviceProvider, UserConverter voterConverter,
        CommentConverter commentConverter)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _voterConverter = voterConverter;
        _commentConverter = commentConverter;
    }

    // Entity

    public CommentVoteEntity ConvertToEntity(CommentVote commentVote)
    {
        if (commentVote == null)
            return null;

        UserEntity voterEntity = null;
        if (commentVote.Voter != null)
            voterEntity = _voterConverter
                .ConvertToEntity(commentVote.Voter.PureCopy());

        CommentEntity commentEntity = null;
        if (commentVote.Comment != null)
            commentEntity = _commentConverter
                .ConvertToEntity(commentVote.Comment.PureCopy());

        var commentVoteEntity = new CommentVoteEntity(
            commentVote.Id, commentVote.VoterId,
            commentVote.VoterUsername, commentVote.CommentId,
            commentVote.IsUp, commentVote.CreationDate,
            commentVote.ModifiedDate, voterEntity, commentEntity);

        return commentVoteEntity;
    }

    public CommentVote ConvertToModel(CommentVoteEntity commentVoteEntity)
    {
        if (commentVoteEntity == null)
            return null;

        User voter = null;
        if (commentVoteEntity.VoterEntity != null)
            voter = _voterConverter.ConvertToModel(commentVoteEntity.VoterEntity.PureCopy());

        Comment comment = null;
        if (commentVoteEntity.CommentEntity != null)
            comment = _commentConverter
                .ConvertToModel(commentVoteEntity.CommentEntity.PureCopy());

        var commentVote = new CommentVote(commentVoteEntity.Id,
            commentVoteEntity.VoterEntityId, commentVoteEntity.VoterEntityUsername,
            commentVoteEntity.CommentEntityId, commentVoteEntity.IsUp,
            commentVoteEntity.CreationDate, commentVoteEntity.ModifiedDate,
            voter, comment);

        return commentVote;
    }
}
