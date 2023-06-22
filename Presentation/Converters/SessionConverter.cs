using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.ValueObjects;

namespace FireplaceApi.Presentation.Converters;

public static class SessionConverter
{
    public static SessionDto ToDto(this Session session)
    {
        if (session == null)
            return null;

        var sessionDto = new SessionDto(session.Id.IdEncode(), session.UserId.IdEncode(),
            session.IpAddress.ToString(), session.CreationDate);

        return sessionDto;
    }

    public static QueryResultDto<SessionDto> ToDto(this QueryResult<Session> queryResult)
        => queryResult.ToDto(ToDto);
}
