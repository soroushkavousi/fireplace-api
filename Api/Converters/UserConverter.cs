using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Api.Converters
{
    public class UserConverter : BaseConverter<User, UserDto>
    {
        private readonly ILogger<UserConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public UserConverter(ILogger<UserConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override UserDto ConvertToDto(User user)
        {
            if (user == null)
                return null;

            EmailDto emailDto = null;
            if (user.Email != null)
                emailDto = _serviceProvider.GetService<EmailConverter>().ConvertToDto(user.Email.PureCopy());

            string accessTokenValue = null;
            if (user.AccessTokens != null && user.AccessTokens.Count != 0)
                accessTokenValue = user.AccessTokens.Last().Value;

            List<SessionDto> sessionDtos = null;
            if (user.Sessions != null && user.Sessions.Count != 0)
                sessionDtos = user.Sessions.Select(
                    session => _serviceProvider.GetService<SessionConverter>().ConvertToDto(session.PureCopy())).ToList();

            var userDto = new UserDto(user.Id.IdEncode(), user.Username, user.State.ToString(),
                user.CreationDate, user.DisplayName, user.About, user.AvatarUrl, user.BannerUrl,
                accessTokenValue, emailDto, sessionDtos);

            return userDto;
        }
    }
}
