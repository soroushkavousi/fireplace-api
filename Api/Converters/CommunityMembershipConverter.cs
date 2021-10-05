using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using FireplaceApi.Core.Models;
using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Operators;

namespace FireplaceApi.Api.Converters
{
    public class CommunityMembershipConverter
    {
        private readonly ILogger<CommunityMembershipConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommunityMembershipConverter(ILogger<CommunityMembershipConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public CommunityMembershipDto ConvertToDto(CommunityMembership communityMembership)
        {
            if (communityMembership == null)
                return null;

            UserDto userDto = null;
            if (communityMembership.User != null)
                userDto = _serviceProvider.GetService<UserConverter>()
                    .ConvertToDto(communityMembership.User.PureCopy());

            CommunityDto communityDto = null;
            if (communityMembership.Community != null)
                communityDto = _serviceProvider.GetService<CommunityConverter>()
                    .ConvertToDto(communityMembership.Community.PureCopy());

            var communityMembershipDto = new CommunityMembershipDto(communityMembership.Id, 
                communityMembership.UserId, communityMembership.Username, 
                communityMembership.CommunityId, communityMembership.CommunityName,
                communityMembership.CreationDate, userDto, communityDto);

            return communityMembershipDto;
        }

        public PageDto<CommunityMembershipDto> ConvertToDto(Page<CommunityMembership> page, string listRelativePath)
        {
            if (page == null)
                return null;

            var listPath = $"{GlobalOperator.GlobalValues.Api.BaseUrlPath}{listRelativePath}";
            
            var paginationDto = new PaginationDto(page.QueryResultPointer, 
                listPath, page.Number, page.Start, page.End, page.Limit,
                page.TotalItemsCount, page.TotalPagesCount);

            var itemDtos = page.Items.Select(communityMembership => ConvertToDto(communityMembership)).ToList();

            var pageDto = new PageDto<CommunityMembershipDto>(itemDtos, paginationDto);
            return pageDto;
        }
    }
}
