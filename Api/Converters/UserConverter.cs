using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using GamingCommunityApi.Api.Controllers.Parameters.UserParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Api.Extensions;
using GamingCommunityApi.Api.Controllers.Parameters.EmailParameters;
using GamingCommunityApi.Api.Controllers.Parameters.SessionParameters;
using GamingCommunityApi.Core.Models.UserInformations;

namespace GamingCommunityApi.Api.Converters
{
    public class UserConverter
    {
        private readonly ILogger<UserConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public UserConverter(ILogger<UserConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public UserDto ConvertToDto(User user)
        {
            if (user == null)
                return null;

            EmailDto emailDto = null;
            if (user.Email != null)
                emailDto = _serviceProvider.GetService<EmailConverter>().ConvertToDto(user.Email.PureCopy());

            string accessTokenValue = null;
            if(user.AccessTokens != null && user.AccessTokens.Count != 0)
                accessTokenValue = user.AccessTokens.Last().Value;

            List<SessionDto> sessionDtos = null;
            if (user.Sessions != null && user.Sessions.Count != 0)
                sessionDtos = user.Sessions.Select(
                    session => _serviceProvider.GetService<SessionConverter>().ConvertToDto(session.PureCopy())).ToList();

            var userDto = new UserDto(user.Id, user.FirstName, user.LastName,
                user.Username, user.State.ToString(), emailDto, 
                accessTokenValue, sessionDtos);

            return userDto;
        }
    }
}
