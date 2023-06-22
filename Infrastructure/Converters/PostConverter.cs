using FireplaceApi.Application.Models;
using FireplaceApi.Infrastructure.Entities;

namespace FireplaceApi.Infrastructure.Converters;

public static class PostConverter
{
    public static PostEntity ToEntity(this Post post)
    {
        if (post == null)
            return null;

        UserEntity authorEntity = null;
        if (post.Author != null)
            authorEntity = post.Author.PureCopy().ToEntity();

        CommunityEntity communityEntity = null;
        if (post.Community != null)
            communityEntity = post.Community.PureCopy().ToEntity();

        var postEntity = new PostEntity(post.Id, post.AuthorId,
            post.AuthorUsername, post.CommunityId,
            post.CommunityName, post.Content,
            post.Vote, post.RequestingUserVote,
            post.CreationDate, post.ModifiedDate,
            authorEntity, communityEntity);

        return postEntity;
    }

    public static Post ToModel(this PostEntity postEntity)
    {
        if (postEntity == null)
            return null;

        User author = null;
        if (postEntity.AuthorEntity != null)
            author = postEntity.AuthorEntity.PureCopy().ToModel();

        Community community = null;
        if (postEntity.CommunityEntity != null)
            community = postEntity.CommunityEntity.PureCopy().ToModel();

        var post = new Post(postEntity.Id,
            postEntity.AuthorEntityId, postEntity.AuthorEntityUsername,
            postEntity.CommunityEntityId, postEntity.CommunityEntityName,
            postEntity.Vote, postEntity.RequestingUserVote,
            postEntity.Content, postEntity.CreationDate,
            postEntity.ModifiedDate, author, community);

        return post;
    }
}
