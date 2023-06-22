using FireplaceApi.Domain.Users;
using System.Collections.Generic;

namespace FireplaceApi.Presentation.Auth;

public record RequestingUser(
    ulong? Id,
    ulong? SessionId,
    UserState? State,
    List<UserRole> Roles
    );
