using FireplaceApi.Domain.Common;
using FireplaceApi.Domain.Users;
using System.Net;

namespace FireplaceApi.Domain.Sessions;

public class Session : BaseModel
{
    public ulong UserId { get; set; }
    public IPAddress IpAddress { get; set; }
    public SessionState State { get; set; }
    public string RefreshToken { get; set; }
    public User User { get; set; }

    public Session(ulong id, ulong userId, IPAddress ipAddress,
        SessionState state, string refreshToken, DateTime creationDate,
        DateTime? modifiedDate = null, User user = null) : base(id, creationDate, modifiedDate)
    {
        IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        State = state;
        RefreshToken = refreshToken;
        UserId = userId;
        User = user;
    }

    public Session PureCopy() => new(Id, UserId, IpAddress,
        State, RefreshToken, CreationDate, ModifiedDate);

    public void RemoveLoopReferencing()
    {
        User = User?.PureCopy();
    }
}