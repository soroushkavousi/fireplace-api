using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters
{
    public class CommunityConverter
    {
        private readonly ILogger<CommunityConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommunityConverter(ILogger<CommunityConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        // Entity

        public CommunityEntity ConvertToEntity(Community community)
        {
            if (community == null)
                return null;

            UserEntity creatorEntity = null;
            if (community.Creator != null)
                creatorEntity = _serviceProvider.GetService<UserConverter>()
                    .ConvertToEntity(community.Creator.PureCopy());

            var communityEntity = new CommunityEntity(community.Id, community.Name,
                community.CreatorId, community.CreatorUsername, community.CreationDate,
                community.ModifiedDate, creatorEntity);

            return communityEntity;
        }

        public Community ConvertToModel(CommunityEntity communityEntity)
        {
            if (communityEntity == null)
                return null;

            User creator = null;
            if (communityEntity.CreatorEntity != null)
                creator = _serviceProvider.GetService<UserConverter>().ConvertToModel(communityEntity.CreatorEntity.PureCopy());

            var community = new Community(communityEntity.Id, communityEntity.Name,
                communityEntity.CreatorEntityId, communityEntity.CreatorEntityUsername,
                communityEntity.CreationDate, communityEntity.ModifiedDate, creator);

            return community;
        }
    }
}
