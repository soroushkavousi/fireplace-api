using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;

namespace FireplaceApi.Infrastructure.Converters;

public static class CommentVoteConverter
{
    // Entity

    public static CommentVoteEntity ToEntity(this CommentVote commentVote)
    {
        if (commentVote == null)
            return null;

        UserEntity voterEntity = null;
        if (commentVote.Voter != null)
            voterEntity = commentVote.Voter.PureCopy().ToEntity();

        CommentEntity commentEntity = null;
        if (commentVote.Comment != null)
            commentEntity = commentVote.Comment.PureCopy().ToEntity();

        var commentVoteEntity = new CommentVoteEntity(
            commentVote.Id, commentVote.VoterId,
            commentVote.VoterUsername, commentVote.CommentId,
            commentVote.IsUp, commentVote.CreationDate,
            commentVote.ModifiedDate, voterEntity, commentEntity);

        return commentVoteEntity;
    }

    public static CommentVote ToModel(this CommentVoteEntity commentVoteEntity)
    {
        if (commentVoteEntity == null)
            return null;

        User voter = null;
        if (commentVoteEntity.VoterEntity != null)
            voter = commentVoteEntity.VoterEntity.PureCopy().ToModel();

        Comment comment = null;
        if (commentVoteEntity.CommentEntity != null)
            comment = commentVoteEntity.CommentEntity.PureCopy().ToModel();

        var commentVote = new CommentVote(commentVoteEntity.Id,
            commentVoteEntity.VoterEntityId, commentVoteEntity.VoterEntityUsername,
            commentVoteEntity.CommentEntityId, commentVoteEntity.IsUp,
            commentVoteEntity.CreationDate, commentVoteEntity.ModifiedDate,
            voter, comment);

        return commentVote;
    }
}
