using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace FireplaceApi.Application.Converters;

public class PostConverter : BaseConverter<Post, PostDto>
{
    private readonly ILogger<PostConverter> _logger;
    private readonly IServiceProvider _serviceProvider;

    public PostConverter(ILogger<PostConverter> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override PostDto ConvertToDto(Post post)
    {
        if (post == null)
            return null;

        ProfileDto profileDto = null;
        if (post.Author != null)
            profileDto = _serviceProvider.GetService<UserConverter>()
                .ConvertToProfileDto(post.Author.PureCopy());

        CommunityDto communityDto = null;
        if (post.Community != null)
            communityDto = _serviceProvider.GetService<CommunityConverter>()
                .ConvertToDto(post.Community.PureCopy());

        QueryResultDto<CommentDto> commentDtos = null;
        if (post.Comments != null)
        {
            post.Comments.Items = post.Comments.Items
                .Select(comment => comment.PureCopy()).ToList();
            commentDtos = _serviceProvider.GetService<CommentConverter>()
                .ConvertToDto(post.Comments);
        }

        var postDto = new PostDto(post.Id.IdEncode(),
            post.AuthorId.IdEncode(), post.AuthorUsername,
            post.CommunityId.IdEncode(), post.CommunityName,
            post.Vote, post.RequestingUserVote, post.Content,
            post.CreationDate, post.ModifiedDate, profileDto,
            communityDto, commentDtos);

        return postDto;
    }
}
