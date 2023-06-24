using FireplaceApi.Application.Users;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.GraphQL;

[ExtendObjectType(typeof(GraphQLQuery))]
public class UserQueryResolvers
{
    [AllowAnonymous]
    public async Task<UserDto> GetMeAsync(
        [Service(ServiceKind.Resolver)] UserService userService,
        [Service(ServiceKind.Resolver)] Validators.UserValidator userValidator,
        [User] RequestingUser requestingUser)
    {
        var user = await userService.GetRequestingUserAsync(requestingUser.Id.Value, true, true);
        var userDto = user.ToDto();
        return userDto;
    }
}
