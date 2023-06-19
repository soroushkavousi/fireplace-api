using FireplaceApi.Domain.Enums;
using System.Collections.Generic;

namespace FireplaceApi.Application.Auth;

public record RequestingUser(
    ulong? Id,
    ulong? SessionId,
    UserState? State,
    List<UserRole> Roles
    );
