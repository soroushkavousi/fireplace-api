using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Tools;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ProfileDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public string DisplayName { get; set; }
        public string About { get; set; }
        public string AvatarUrl { get; set; }
        public string BannerUrl { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Username).ToSnakeCase()] = UserDto.PureExample1[nameof(Username).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = UserDto.PureExample1[nameof(CreationDate).ToSnakeCase()],
            [nameof(DisplayName).ToSnakeCase()] = UserDto.PureExample1[nameof(DisplayName).ToSnakeCase()],
            [nameof(About).ToSnakeCase()] = UserDto.PureExample1[nameof(About).ToSnakeCase()],
            [nameof(AvatarUrl).ToSnakeCase()] = UserDto.PureExample1[nameof(AvatarUrl).ToSnakeCase()],
            [nameof(BannerUrl).ToSnakeCase()] = UserDto.PureExample1[nameof(BannerUrl).ToSnakeCase()],
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Username).ToSnakeCase()] = UserDto.PureExample2[nameof(Username).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = UserDto.PureExample2[nameof(CreationDate).ToSnakeCase()],
            [nameof(DisplayName).ToSnakeCase()] = UserDto.PureExample2[nameof(DisplayName).ToSnakeCase()],
            [nameof(About).ToSnakeCase()] = UserDto.PureExample2[nameof(About).ToSnakeCase()],
            [nameof(AvatarUrl).ToSnakeCase()] = UserDto.PureExample2[nameof(AvatarUrl).ToSnakeCase()],
            [nameof(BannerUrl).ToSnakeCase()] = UserDto.PureExample2[nameof(BannerUrl).ToSnakeCase()],
        };

        public static OpenApiArray PureListExample1 { get; } = new OpenApiArray
        {
            PureExample1, PureExample2
        };

        public static OpenApiObject Example1 { get; } = new OpenApiObject
        {
            [nameof(Username).ToSnakeCase()] = PureExample1[nameof(Username).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample1[nameof(CreationDate).ToSnakeCase()],
            [nameof(DisplayName).ToSnakeCase()] = PureExample1[nameof(DisplayName).ToSnakeCase()],
            [nameof(About).ToSnakeCase()] = PureExample1[nameof(About).ToSnakeCase()],
            [nameof(AvatarUrl).ToSnakeCase()] = PureExample1[nameof(AvatarUrl).ToSnakeCase()],
            [nameof(BannerUrl).ToSnakeCase()] = PureExample1[nameof(BannerUrl).ToSnakeCase()],
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(Username).ToSnakeCase()] = PureExample2[nameof(Username).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = PureExample2[nameof(CreationDate).ToSnakeCase()],
            [nameof(DisplayName).ToSnakeCase()] = PureExample2[nameof(DisplayName).ToSnakeCase()],
            [nameof(About).ToSnakeCase()] = PureExample2[nameof(About).ToSnakeCase()],
            [nameof(AvatarUrl).ToSnakeCase()] = PureExample2[nameof(AvatarUrl).ToSnakeCase()],
            [nameof(BannerUrl).ToSnakeCase()] = PureExample2[nameof(BannerUrl).ToSnakeCase()],
        };
        public static OpenApiArray ListExample1 { get; } = new OpenApiArray
        {
            Example1, Example2
        };

        public static IOpenApiAny Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(UserController.GetUserProfileAsync)] = Example1,
        };

        static ProfileDto()
        {

        }

        public ProfileDto(string username, DateTime creationDate,
            string displayName, string about, string avatarUrl, string bannerUrl)
        {
            Username = username;
            CreationDate = creationDate;
            DisplayName = displayName;
            About = about;
            AvatarUrl = avatarUrl;
            BannerUrl = bannerUrl;
        }
    }
}
