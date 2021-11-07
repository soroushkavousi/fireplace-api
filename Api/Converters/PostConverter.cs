using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Api.Converters
{
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

            UserDto authorDto = null;
            if (post.Author != null)
                authorDto = _serviceProvider.GetService<UserConverter>()
                    .ConvertToDto(post.Author.PureCopy());

            CommunityDto communityDto = null;
            if (post.Community != null)
                communityDto = _serviceProvider.GetService<CommunityConverter>()
                    .ConvertToDto(post.Community.PureCopy());

            var postDto = new PostDto(post.Id.Encode(),
                post.AuthorId.Encode(), post.AuthorUsername,
                post.CommunityId.Encode(), post.CommunityName,
                post.Vote, post.RequesterUserVote, post.Content,
                post.CreationDate, post.ModifiedDate, authorDto, communityDto);

            return postDto;
        }
    }
}
