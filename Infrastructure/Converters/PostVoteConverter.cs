using FireplaceApi.Domain.Posts;
using FireplaceApi.Domain.Users;
using FireplaceApi.Infrastructure.Entities;

namespace FireplaceApi.Infrastructure.Converters;

public static class PostVoteConverter
{
    public static PostVoteEntity ToEntity(this PostVote postVote)
    {
        if (postVote == null)
            return null;

        UserEntity voterEntity = null;
        if (postVote.Voter != null)
            voterEntity = postVote.Voter.PureCopy().ToEntity();

        PostEntity postEntity = null;
        if (postVote.Post != null)
            postEntity = postVote.Post.PureCopy().ToEntity();

        var postVoteEntity = new PostVoteEntity(postVote.Id,
            postVote.VoterId, postVote.VoterUsername.Value, postVote.PostId,
            postVote.IsUp, postVote.CreationDate,
            postVote.ModifiedDate, voterEntity, postEntity);

        return postVoteEntity;
    }

    public static PostVote ToModel(this PostVoteEntity postVoteEntity)
    {
        if (postVoteEntity == null)
            return null;

        User voter = null;
        if (postVoteEntity.VoterEntity != null)
            voter = postVoteEntity.VoterEntity.PureCopy().ToModel();

        Post post = null;
        if (postVoteEntity.PostEntity != null)
            post = postVoteEntity.PostEntity.PureCopy().ToModel();

        var postVote = new PostVote(postVoteEntity.Id,
            postVoteEntity.VoterEntityId, new Username(postVoteEntity.VoterEntityUsername),
            postVoteEntity.PostEntityId, postVoteEntity.IsUp,
            postVoteEntity.CreationDate, postVoteEntity.ModifiedDate,
            voter, post);

        return postVote;
    }
}
