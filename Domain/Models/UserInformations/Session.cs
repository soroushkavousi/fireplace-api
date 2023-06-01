using FireplaceApi.Domain.Enums;
using System;
using System.Net;

namespace FireplaceApi.Domain.Models;

public class Session : BaseModel
{
    public ulong UserId { get; set; }
    public IPAddress IpAddress { get; set; }
    public SessionState State { get; set; }
    public User User { get; set; }

    public Session(ulong id, ulong userId, IPAddress ipAddress,
        SessionState state, DateTime creationDate, DateTime? modifiedDate = null,
        User user = null) : base(id, creationDate, modifiedDate)
    {
        IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        State = state;
        UserId = userId;
        User = user;
    }

    public Session PureCopy() => new(Id, UserId, IpAddress,
        State, CreationDate, ModifiedDate);

    public void RemoveLoopReferencing()
    {
        User = User?.PureCopy();
    }
}