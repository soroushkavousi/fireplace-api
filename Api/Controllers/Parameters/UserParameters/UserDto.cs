using Microsoft.OpenApi.Any;
using GamingCommunityApi.Api.Controllers.Parameters.EmailParameters;
using GamingCommunityApi.Api.Extensions;
using GamingCommunityApi.Api.Interfaces;
using GamingCommunityApi.Api.Tools.Swagger;
using GamingCommunityApi.Api.Tools.Swagger.SchemaFilters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Api.Controllers.Parameters.SessionParameters;
using GamingCommunityApi.Api.Controllers.Parameters.AccessTokenParameters;
using GamingCommunityApi.Core.Extensions;

namespace GamingCommunityApi.Api.Controllers.Parameters.UserParameters
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class UserDto
    {
        [Required]
        public long? Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public EmailDto Email { get; set; }
        public string AccessToken { get; set; }
        public List<SessionDto> Sessions { get; set; }

        public static OpenApiObject PureUserExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(1000000),
            [nameof(FirstName).ToSnakeCase()] = new OpenApiString("Ted"),
            [nameof(LastName).ToSnakeCase()] = new OpenApiString("Mosby"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("tedmosby"),
            [nameof(State).ToSnakeCase()] = new OpenApiString(Core.Enums.UserState.VERIFIED.ToString()),
            [nameof(Email).ToSnakeCase()] = new OpenApiNull(),
            [nameof(AccessToken).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Sessions).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureUserExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(2000000),
            [nameof(FirstName).ToSnakeCase()] = new OpenApiString("Barney"),
            [nameof(LastName).ToSnakeCase()] = new OpenApiString("Stinson"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("barneystinson"),
            [nameof(State).ToSnakeCase()] = new OpenApiString(Core.Enums.UserState.NOT_VERIFIED.ToString()),
            [nameof(Email).ToSnakeCase()] = new OpenApiNull(),
            [nameof(AccessToken).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Sessions).ToSnakeCase()] = new OpenApiNull(),
        };
        
        public static OpenApiArray ListOfPureUsersExample1 { get; } = new OpenApiArray
        {
            PureUserExample1, PureUserExample2
        };

        public static OpenApiObject UserExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureUserExample1[nameof(Id).ToSnakeCase()],
            [nameof(FirstName).ToSnakeCase()] = PureUserExample1[nameof(FirstName).ToSnakeCase()],
            [nameof(LastName).ToSnakeCase()] = PureUserExample1[nameof(LastName).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = PureUserExample1[nameof(Username).ToSnakeCase()],
            [nameof(State).ToSnakeCase()] = PureUserExample1[nameof(State).ToSnakeCase()],
            [nameof(Email).ToSnakeCase()] = EmailDto.PureEmailExample1,
            [nameof(AccessToken).ToSnakeCase()] = AccessTokenDto.PureAccessTokenExample1[nameof(AccessTokenDto.Value).ToSnakeCase()],
            [nameof(Sessions).ToSnakeCase()] = SessionDto.ListOfPureSessionsExample1,
        };
        public static OpenApiObject UserExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureUserExample2[nameof(Id).ToSnakeCase()],
            [nameof(FirstName).ToSnakeCase()] = PureUserExample2[nameof(FirstName).ToSnakeCase()],
            [nameof(LastName).ToSnakeCase()] = PureUserExample2[nameof(LastName).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = PureUserExample2[nameof(Username).ToSnakeCase()],
            [nameof(State).ToSnakeCase()] = PureUserExample2[nameof(State).ToSnakeCase()],
            [nameof(Email).ToSnakeCase()] = EmailDto.PureEmailExample2,
            [nameof(AccessToken).ToSnakeCase()] = AccessTokenDto.PureAccessTokenExample2[nameof(AccessTokenDto.Value).ToSnakeCase()],
            [nameof(Sessions).ToSnakeCase()] = SessionDto.ListOfPureSessionsExample2,
        };
        public static OpenApiArray ListOfUsersExample1 { get; } = new OpenApiArray
        {
            UserExample1, UserExample2
        };

        public static IOpenApiAny Example { get; } = UserExample1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(UserController.SignUpWithEmailAsync)] = UserExample1,
            [nameof(UserController.LogInWithEmailAsync)] = UserExample1,
            [nameof(UserController.LogInWithUsernameAsync)] = UserExample1,
            [nameof(UserController.ListUsersAsync)] = ListOfUsersExample1,
            [nameof(UserController.GetUserByIdAsync)] = UserExample1,
            [nameof(UserController.PatchUserAsync)] = UserExample1,
            [nameof(UserController.DeleteUserAsync)] = UserExample1,
        };

        static UserDto()
        {

        }

        public UserDto(long? id, string firstName, string lastName, 
            string username, string state, EmailDto email, 
            string accessToken = null, List<SessionDto> sessions = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            State = state;
            Email = email;
            AccessToken = accessToken;
            Sessions = sessions;
        }
    }
}
