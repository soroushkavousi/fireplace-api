﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Enums;

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

            var communityEntity = new CommunityEntity(community.Name, 
                community.CreatorId, community.CreationDate, community.ModifiedDate,
                community.Id, creatorEntity);

            return communityEntity;
        }

        public Community ConvertToModel(CommunityEntity communityEntity)
        {
            if (communityEntity == null)
                return null;

            User creator = null;
            if (communityEntity.CreatorEntity != null)
                creator = _serviceProvider.GetService<UserConverter>().ConvertToModel(communityEntity.CreatorEntity.PureCopy());

            var community = new Community(communityEntity.Id.Value, communityEntity.Name, 
                communityEntity.CreatorEntityId, communityEntity.CreationDate, 
                communityEntity.ModifiedDate, creator);

            return community;
        }
    }
}