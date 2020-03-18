using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Entities;
using GamingCommunityApi.Entities.UserInformationEntities;
using GamingCommunityApi.Models;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Controllers.Parameters.EmailParameters;
using GamingCommunityApi.ValueObjects;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Controllers.Parameters.UserParameters;

namespace GamingCommunityApi.Converters
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

        // Dto

        public EmailDto ConvertToDto(Email email)
        {
            if (email == null)
                return null;

            UserDto userDto = null;
            if (email.User != null)
                userDto = _serviceProvider.GetService<UserConverter>().ConvertToDto(email.User.PureCopy());

            var emailDto = new EmailDto(email.Id, email.UserId, email.Address,
                email.Activation.Status.ToString(), userDto);

            return emailDto;
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
                email.Activation.Code, email.Activation.Status.ToString(), 
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

            var activation = new Activation(emailEntity.ActivationCode,
                emailEntity.ActivationStatus.ToEnum<ActivationStatus>());

            var email = new Email(emailEntity.Id.Value, emailEntity.UserEntityId, emailEntity.Address,
                activation, user);

            return email;
        }
    }
}
