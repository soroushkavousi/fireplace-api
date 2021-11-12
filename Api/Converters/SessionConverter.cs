using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Api.Converters
{
    public class SessionConverter : BaseConverter<Session, SessionDto>
    {
        private readonly ILogger<SessionConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SessionConverter(ILogger<SessionConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override SessionDto ConvertToDto(Session session)
        {
            if (session == null)
                return null;

            UserDto userDto = null;
            if (session.User != null)
                userDto = _serviceProvider.GetService<UserConverter>()
                    .ConvertToDto(session.User.PureCopy());

            var sessionDto = new SessionDto(session.Id.IdEncode(), session.UserId.IdEncode(),
                session.IpAddress.ToString(), session.CreationDate, userDto);

            return sessionDto;
        }
    }
}
