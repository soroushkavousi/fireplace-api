using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class UserDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public string AccessToken { get; set; }
        public EmailDto Email { get; set; }
        public List<SessionDto> Sessions { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiString("5FhKGzw165x"),
            [nameof(FirstName).ToSnakeCase()] = new OpenApiString("Ted"),
            [nameof(LastName).ToSnakeCase()] = new OpenApiString("Mosby"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("tedmosby"),
            [nameof(State).ToSnakeCase()] = new OpenApiString(Core.Enums.UserState.VERIFIED.ToString()),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(AccessToken).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Email).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Sessions).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiString("fNnXz8tyV1b"),
            [nameof(FirstName).ToSnakeCase()] = new OpenApiString("Barney"),
            [nameof(LastName).ToSnakeCase()] = new OpenApiString("Stinson"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("barneystinson"),
            [nameof(State).ToSnakeCase()] = new OpenApiString(Core.Enums.UserState.NOT_VERIFIED.ToString()),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(AccessToken).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Email).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Sessions).ToSnakeCase()] = new OpenApiNull(),
        };

        public static OpenApiArray PureListExample1 { get; } = new OpenApiArray
        {
            PureExample1, PureExample2
        };

        public static OpenApiObject Example1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample1[nameof(Id).ToSnakeCase()],
            [nameof(FirstName).ToSnakeCase()] = PureExample1[nameof(FirstName).ToSnakeCase()],
            [nameof(LastName).ToSnakeCase()] = PureExample1[nameof(LastName).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = PureExample1[nameof(Username).ToSnakeCase()],
            [nameof(State).ToSnakeCase()] = PureExample1[nameof(State).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(AccessToken).ToSnakeCase()] = AccessTokenDto.PureExample1[nameof(AccessTokenDto.Value).ToSnakeCase()],
            [nameof(Email).ToSnakeCase()] = EmailDto.PureExample1,
            [nameof(Sessions).ToSnakeCase()] = SessionDto.PureListExample1,
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
            [nameof(FirstName).ToSnakeCase()] = PureExample2[nameof(FirstName).ToSnakeCase()],
            [nameof(LastName).ToSnakeCase()] = PureExample2[nameof(LastName).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = PureExample2[nameof(Username).ToSnakeCase()],
            [nameof(State).ToSnakeCase()] = PureExample2[nameof(State).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(AccessToken).ToSnakeCase()] = AccessTokenDto.PureExample2[nameof(AccessTokenDto.Value).ToSnakeCase()],
            [nameof(Email).ToSnakeCase()] = EmailDto.PureExample2,
            [nameof(Sessions).ToSnakeCase()] = SessionDto.PureListExample2,
        };
        public static OpenApiArray ListExample1 { get; } = new OpenApiArray
        {
            Example1, Example2
        };

        public static IOpenApiAny Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(UserController.OpenGoogleLogInPage)] = Example1,
            [nameof(UserController.LogInWithGoogleAsync)] = Example1,
            [nameof(UserController.SignUpWithEmailAsync)] = Example1,
            [nameof(UserController.LogInWithEmailAsync)] = Example1,
            [nameof(UserController.LogInWithUsernameAsync)] = Example1,
            [nameof(UserController.GetRequestingUserAsync)] = Example1,
            [nameof(UserController.GetUserByEncodedIdOrUsernameAsync)] = Example1,
            [nameof(UserController.PatchRequestingUserAsync)] = Example1,
            [nameof(UserController.DeleteRequestingUserAsync)] = new OpenApiNull(),
        };

        static UserDto()
        {

        }

        public UserDto(string id, string firstName, string lastName,
            string username, string state, DateTime creationDate,
            string accessToken = null, EmailDto email = null,
            List<SessionDto> sessions = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            State = state;
            CreationDate = creationDate;
            AccessToken = accessToken;
            Email = email;
            Sessions = sessions;
        }
    }
}
