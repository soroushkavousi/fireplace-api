using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.Converters;
using FireplaceApi.Application.Tool;
using FireplaceApi.Application.Validators;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Services;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Resolvers;

[ExtendObjectType(typeof(GraphQLQuery))]
public class UserQueryResolvers
{
    [AllowAnonymous]
    public async Task<UserDto> GetMeAsync(
        [Service(ServiceKind.Resolver)] UserService userService,
        [Service(ServiceKind.Resolver)] UserValidator userValidator,
        [User] User requestingUser)
    {
        var user = await userService.GetRequestingUserAsync(requestingUser, true, true);
        var userDto = user.ToDto();
        return userDto;
    }
}
