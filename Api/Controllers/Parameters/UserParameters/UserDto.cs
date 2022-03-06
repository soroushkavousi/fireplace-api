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
        public string Username { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public string DisplayName { get; set; }
        public string About { get; set; }
        public string AvatarUrl { get; set; }
        public string BannerUrl { get; set; }
        public string AccessToken { get; set; }
        public EmailDto Email { get; set; }
        public List<SessionDto> Sessions { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiString("5FhKGzw165x"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("erenyeager"),
            [nameof(State).ToSnakeCase()] = new OpenApiString(Core.Enums.UserState.VERIFIED.ToString()),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(DisplayName).ToSnakeCase()] = new OpenApiString("Eren Yeager"),
            [nameof(About).ToSnakeCase()] = new OpenApiString("ABOUT ME!"),
            [nameof(AvatarUrl).ToSnakeCase()] = new OpenApiString("https://.../avatar.png"),
            [nameof(BannerUrl).ToSnakeCase()] = new OpenApiString("https://.../banner.png"),
            [nameof(AccessToken).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Email).ToSnakeCase()] = new OpenApiNull(),
            [nameof(Sessions).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiString("fNnXz8tyV1b"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("lelouchlamperouge"),
            [nameof(State).ToSnakeCase()] = new OpenApiString(Core.Enums.UserState.NOT_VERIFIED.ToString()),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(DisplayName).ToSnakeCase()] = new OpenApiString("Lelouch Lamperouge"),
            [nameof(About).ToSnakeCase()] = new OpenApiString("ABOUT ME!"),
            [nameof(AvatarUrl).ToSnakeCase()] = new OpenApiString("https://.../avatar.png"),
            [nameof(BannerUrl).ToSnakeCase()] = new OpenApiString("https://.../banner.png"),
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
            [nameof(Username).ToSnakeCase()] = PureExample1[nameof(Username).ToSnakeCase()],
            [nameof(State).ToSnakeCase()] = PureExample1[nameof(State).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample1[nameof(CreationDate).ToSnakeCase()],
            [nameof(DisplayName).ToSnakeCase()] = PureExample1[nameof(DisplayName).ToSnakeCase()],
            [nameof(About).ToSnakeCase()] = PureExample1[nameof(About).ToSnakeCase()],
            [nameof(AvatarUrl).ToSnakeCase()] = PureExample1[nameof(AvatarUrl).ToSnakeCase()],
            [nameof(BannerUrl).ToSnakeCase()] = PureExample1[nameof(BannerUrl).ToSnakeCase()],
            [nameof(AccessToken).ToSnakeCase()] = new OpenApiString("e207d1b29e9146a2b143cb1a6e3aaa26"),
            [nameof(Email).ToSnakeCase()] = EmailDto.PureExample1,
            [nameof(Sessions).ToSnakeCase()] = SessionDto.PureListExample1,
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
            [nameof(Username).ToSnakeCase()] = PureExample2[nameof(Username).ToSnakeCase()],
            [nameof(State).ToSnakeCase()] = PureExample2[nameof(State).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample2[nameof(CreationDate).ToSnakeCase()],
            [nameof(DisplayName).ToSnakeCase()] = PureExample2[nameof(DisplayName).ToSnakeCase()],
            [nameof(About).ToSnakeCase()] = PureExample2[nameof(About).ToSnakeCase()],
            [nameof(AvatarUrl).ToSnakeCase()] = PureExample2[nameof(AvatarUrl).ToSnakeCase()],
            [nameof(BannerUrl).ToSnakeCase()] = PureExample2[nameof(BannerUrl).ToSnakeCase()],
            [nameof(AccessToken).ToSnakeCase()] = new OpenApiString("957b25e5ef5c4de68a135eafab380918"),
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

        public UserDto(string id, string username, string state, DateTime creationDate,
            string displayName, string about, string avatarUrl, string bannerUrl,
            string accessToken = null, EmailDto email = null,
            List<SessionDto> sessions = null)
        {
            Id = id;
            Username = username;
            State = state;
            CreationDate = creationDate;
            DisplayName = displayName;
            About = about;
            AvatarUrl = avatarUrl;
            BannerUrl = bannerUrl;
            AccessToken = accessToken;
            Email = email;
            Sessions = sessions;
        }
    }
}
