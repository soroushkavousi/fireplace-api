using Microsoft.OpenApi.Any;
using GamingCommunityApi.Controllers.Parameters.EmailParameters;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Interfaces;
using GamingCommunityApi.Tools.Swagger;
using GamingCommunityApi.Tools.Swagger.SchemaFilters;
using GamingCommunityApi.Tools.TextJsonSerializer;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Controllers.Parameters.SessionParameters;
using GamingCommunityApi.Controllers.Parameters.AccessTokenParameters;

namespace GamingCommunityApi.Controllers.Parameters.UserParameters
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
            [nameof(FirstName).ToSnakeCase()] = new OpenApiString("Joseph"),
            [nameof(LastName).ToSnakeCase()] = new OpenApiString("Armstrong"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("josepharmstrong"),
            [nameof(State).ToSnakeCase()] = new OpenApiString(Enums.UserState.VERIFIED.ToString()),
            [nameof(Email).ToSnakeCase()] = new OpenApiNull(),
            [nameof(AccessToken).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Sessions).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureUserExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(2000000),
            [nameof(FirstName).ToSnakeCase()] = new OpenApiString("James"),
            [nameof(LastName).ToSnakeCase()] = new OpenApiString("Davis"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("jamesdavis"),
            [nameof(State).ToSnakeCase()] = new OpenApiString(Enums.UserState.NOT_VERIFIED.ToString()),
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
