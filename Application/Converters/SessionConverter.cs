using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Application.Converters;

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

        var sessionDto = new SessionDto(session.Id.IdEncode(), session.UserId.IdEncode(),
            session.IpAddress.ToString(), session.CreationDate);

        return sessionDto;
    }
}
