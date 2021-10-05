using Microsoft.AspNetCore.Http;
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
    public class CommunityMembershipConverter
    {
        private readonly ILogger<CommunityMembershipConverter> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserConverter _userConverter;
        private readonly CommunityConverter _communityConverter;

        public CommunityMembershipConverter(ILogger<CommunityMembershipConverter> logger, 
            IServiceProvider serviceProvider, UserConverter userConverter,
            CommunityConverter communityConverter)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _userConverter = userConverter;
            _communityConverter = communityConverter;
        }

        // Entity

        public CommunityMembershipEntity ConvertToEntity(CommunityMembership communityMembership)
        {
            if (communityMembership == null)
                return null;

            UserEntity userEntity = null;
            if (communityMembership.User != null)
                userEntity = _userConverter
                    .ConvertToEntity(communityMembership.User.PureCopy());

            CommunityEntity communityEntity = null;
            if (communityMembership.Community != null)
                communityEntity = _communityConverter
                    .ConvertToEntity(communityMembership.Community.PureCopy());

            var communityMembershipEntity = new CommunityMembershipEntity(communityMembership.UserId, 
                communityMembership.Username, communityMembership.CommunityId, 
                communityMembership.CommunityName, communityMembership.CreationDate, 
                communityMembership.ModifiedDate, communityMembership.Id, userEntity, communityEntity);

            return communityMembershipEntity;
        }

        public CommunityMembership ConvertToModel(CommunityMembershipEntity communityMembershipEntity)
        {
            if (communityMembershipEntity == null)
                return null;

            User user = null;
            if (communityMembershipEntity.UserEntity != null)
                user = _userConverter.ConvertToModel(communityMembershipEntity.UserEntity.PureCopy());

            Community community = null;
            if (communityMembershipEntity.CommunityEntity != null)
                community = _communityConverter
                    .ConvertToModel(communityMembershipEntity.CommunityEntity.PureCopy());

            var communityMembership = new CommunityMembership(communityMembershipEntity.Id.Value, 
                communityMembershipEntity.UserEntityId, communityMembershipEntity.UserEntityName, 
                communityMembershipEntity.CommunityEntityId, communityMembershipEntity.CommunityEntityName,
                communityMembershipEntity.CreationDate, communityMembershipEntity.ModifiedDate, user,
                community);

            return communityMembership;
        }
    }
}
