using Microsoft.OpenApi.Any;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Interfaces;
using FireplaceApi.Api.Tools;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class AccessTokenDto
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public UserDto User { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(Value).ToSnakeCase()] = new OpenApiString("e207d1b29e9146a2b143cb1a6e3aaa26"),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(Value).ToSnakeCase()] = new OpenApiString("957b25e5ef5c4de68a135eafab380918"),
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };

        public static OpenApiObject Example1 { get; } = new OpenApiObject
        {
            [nameof(UserId).ToSnakeCase()] = PureExample1[nameof(UserId).ToSnakeCase()],
            [nameof(Value).ToSnakeCase()] = PureExample1[nameof(Value).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(User).ToSnakeCase()] = UserDto.PureExample1,
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(UserId).ToSnakeCase()] = PureExample2[nameof(UserId).ToSnakeCase()],
            [nameof(Value).ToSnakeCase()] = PureExample2[nameof(Value).ToSnakeCase()],
            [nameof(CreationDate).ToSnakeCase()] = new OpenApiDateTime(Utils.GetYesterdayDate()),
            [nameof(User).ToSnakeCase()] = UserDto.PureExample2,
        };

        public static OpenApiObject Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(AccessTokenController.GetAccessTokenByValueAsync)] = Example1,
        };

        static AccessTokenDto()
        {
            PureExample1[nameof(UserId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()];
            PureExample2[nameof(UserId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()];
        }

        public AccessTokenDto(long userId, string value,
            DateTime creationDate, UserDto user)
        {
            UserId = userId;
            Value = value;
            CreationDate = creationDate;
            User = user;
        }
    }
}
