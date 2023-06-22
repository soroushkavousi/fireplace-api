using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.ValueObjects;
using System.Linq;

namespace FireplaceApi.Presentation.Converters;

public static class PostConverter
{
    public static PostDto ToDto(this Post post)
    {
        if (post == null)
            return null;

        ProfileDto profileDto = null;
        if (post.Author != null)
            profileDto = post.Author.PureCopy().ToProfileDto();

        CommunityDto communityDto = null;
        if (post.Community != null)
            communityDto = post.Community.PureCopy().ToDto();

        QueryResultDto<CommentDto> commentDtos = null;
        if (post.Comments != null)
        {
            post.Comments.Items = post.Comments.Items
                .Select(comment => comment.PureCopy()).ToList();
            commentDtos = post.Comments.ToDto(CommentConverter.ToDto);
        }

        var postDto = new PostDto(post.Id.IdEncode(),
            post.AuthorId.IdEncode(), post.AuthorUsername,
            post.CommunityId.IdEncode(), post.CommunityName,
            post.Vote, post.RequestingUserVote, post.Content,
            post.CreationDate, post.ModifiedDate, profileDto,
            communityDto, commentDtos);

        return postDto;
    }

    public static QueryResultDto<PostDto> ToDto(this QueryResult<Post> queryResult)
        => queryResult.ToDto(ToDto);
}
