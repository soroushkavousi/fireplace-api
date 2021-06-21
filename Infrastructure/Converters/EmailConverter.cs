using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Infrastructure.Entities;
using GamingCommunityApi.Infrastructure.Entities.UserInformationEntities;
using GamingCommunityApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models.UserInformations;
using GamingCommunityApi.Core.ValueObjects;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Enums;

namespace GamingCommunityApi.Infrastructure.Converters
{
    public class EmailConverter
    {
        private readonly ILogger<EmailConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public EmailConverter(ILogger<EmailConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        // Entity

        public EmailEntity ConvertToEntity(Email email)
        {
            if (email == null)
                return null;

            UserEntity userEntity = null;
            if (email.User != null)
                userEntity = _serviceProvider.GetService<UserConverter>().ConvertToEntity(email.User.PureCopy());

            var emailEntity = new EmailEntity(email.UserId, email.Address,
                email.Activation.Status.ToString(), email.Activation.Code, 
                email.Id, userEntity);

            return emailEntity;
        }

        public Email ConvertToModel(EmailEntity emailEntity)
        {
            if (emailEntity == null)
                return null;

            User user = null;
            if (emailEntity.UserEntity != null)
                user = _serviceProvider.GetService<UserConverter>().ConvertToModel(emailEntity.UserEntity.PureCopy());

            var activation = new Activation(emailEntity.ActivationStatus.ToEnum<ActivationStatus>(),
                emailEntity.ActivationCode);

            var email = new Email(emailEntity.Id.Value, emailEntity.UserEntityId, emailEntity.Address,
                activation, user);

            return email;
        }
    }
}
