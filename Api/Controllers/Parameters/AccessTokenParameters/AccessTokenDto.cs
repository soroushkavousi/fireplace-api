using Microsoft.OpenApi.Any;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools.Swagger;
using FireplaceApi.Api.Tools.Swagger.SchemaFilters;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Api.Controllers.Parameters.UserParameters;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Controllers.Parameters.AccessTokenParameters
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class AccessTokenDto
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Value { get; set; }
        public UserDto User { get; set; }

        public static OpenApiObject PureAccessTokenExample1 { get; } = new OpenApiObject
        {
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(Value).ToSnakeCase()] = new OpenApiString("e207d1b29e9146a2b143cb1a6e3aaa26"),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureAccessTokenExample2 { get; } = new OpenApiObject
        {
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(Value).ToSnakeCase()] = new OpenApiString("957b25e5ef5c4de68a135eafab380918"),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };

        public static OpenApiObject AccessTokenExample1 { get; } = new OpenApiObject
        {
            [nameof(UserId).ToSnakeCase()] = PureAccessTokenExample1[nameof(UserId).ToSnakeCase()],
            [nameof(Value).ToSnakeCase()] = PureAccessTokenExample1[nameof(Value).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureUserExample1,
        };
        public static OpenApiObject AccessTokenExample2 { get; } = new OpenApiObject
        {
            [nameof(UserId).ToSnakeCase()] = PureAccessTokenExample2[nameof(UserId).ToSnakeCase()],
            [nameof(Value).ToSnakeCase()] = PureAccessTokenExample2[nameof(Value).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureUserExample2,
        };

        public static OpenApiObject Example { get; } = AccessTokenExample1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(AccessTokenController.GetAccessTokenByValueAsync)] = AccessTokenExample1,
        };

        static AccessTokenDto()
        {
            PureAccessTokenExample1[nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample1[nameof(UserDto.Id).ToSnakeCase()];
            PureAccessTokenExample2[nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample1[nameof(UserDto.Id).ToSnakeCase()];
        }

        public AccessTokenDto(long userId, string value, UserDto user)
        {
            UserId = userId;
            Value = value;
            User = user;
        }
    }
}
