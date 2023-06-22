using FireplaceApi.Domain.Sessions;
using FireplaceApi.Presentation.Dtos;

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
