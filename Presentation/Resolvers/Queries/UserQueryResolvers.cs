using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Presentation.Tools;
using FireplaceApi.Presentation.Validators;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Services;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Resolvers;

[ExtendObjectType(typeof(GraphQLQuery))]
public class UserQueryResolvers
{
    [AllowAnonymous]
    public async Task<UserDto> GetMeAsync(
        [Service(ServiceKind.Resolver)] UserService userService,
        [Service(ServiceKind.Resolver)] UserValidator userValidator,
        [User] RequestingUser requestingUser)
    {
        var user = await userService.GetRequestingUserAsync(requestingUser.Id.Value, true, true);
        var userDto = user.ToDto();
        return userDto;
    }
}
