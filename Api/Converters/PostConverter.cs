using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace FireplaceApi.Api.Converters
{
    public class PostConverter
    {
        private readonly ILogger<PostConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PostConverter(ILogger<PostConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public PostDto ConvertToDto(Post post)
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

            var postDto = new PostDto(post.Id,
                post.AuthorId, post.AuthorUsername,
                post.CommunityId, post.CommunityName,
                post.Vote, post.Content, post.CreationDate,
                post.ModifiedDate, authorDto, communityDto);

            return postDto;
        }

        public PageDto<PostDto> ConvertToDto(Page<Post> page, string listRelativePath)
        {
            if (page == null)
                return null;

            var listPath = $"{GlobalOperator.GlobalValues.Api.BaseUrlPath}{listRelativePath}";

            var paginationDto = new PaginationDto(page.QueryResultPointer,
                listPath, page.Number, page.Start, page.End, page.Limit,
                page.TotalItemsCount, page.TotalPagesCount);

            var itemDtos = page.Items.Select(post => ConvertToDto(post)).ToList();

            var pageDto = new PageDto<PostDto>(itemDtos, paginationDto);
            return pageDto;
        }
    }
}
