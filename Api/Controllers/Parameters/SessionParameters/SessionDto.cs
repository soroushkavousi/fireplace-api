using Microsoft.OpenApi.Any;
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
using GamingCommunityApi.Api.Controllers.Parameters.UserParameters;
using System.Net;
using GamingCommunityApi.Core.Extensions;

namespace GamingCommunityApi.Api.Controllers.Parameters.SessionParameters
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class SessionDto
    {
        [Required]
        public long? Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string IpAddress { get; set; }
        public UserDto User { get; set; }

        public static OpenApiObject PureSessionExample11 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(1000011),
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(IpAddress).ToSnakeCase()] = new OpenApiString("111.111.111.111"),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureSessionExample12 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(1000012),
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(IpAddress).ToSnakeCase()] = new OpenApiString("111.111.111.112"),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureSessionExample21 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(2000011),
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(IpAddress).ToSnakeCase()] = new OpenApiString("222.222.222.222"),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureSessionExample22 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(2000012),
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(IpAddress).ToSnakeCase()] = new OpenApiString("222.222.222.223"),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiArray ListOfPureSessionsExample1 { get; } = new OpenApiArray
        {
            PureSessionExample11, PureSessionExample12
        };
        public static OpenApiArray ListOfPureSessionsExample2 { get; } = new OpenApiArray
        {
            PureSessionExample21, PureSessionExample22
        };

        public static OpenApiObject SessionExample11 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureSessionExample11[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = PureSessionExample11[nameof(UserId).ToSnakeCase()],
            [nameof(IpAddress).ToSnakeCase()] = PureSessionExample11[nameof(IpAddress).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureUserExample1,
        };
        public static OpenApiObject SessionExample12 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureSessionExample12[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = PureSessionExample12[nameof(UserId).ToSnakeCase()],
            [nameof(IpAddress).ToSnakeCase()] = PureSessionExample12[nameof(IpAddress).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureUserExample1,
        };
        public static OpenApiObject SessionExample21 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureSessionExample21[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = PureSessionExample21[nameof(UserId).ToSnakeCase()],
            [nameof(IpAddress).ToSnakeCase()] = PureSessionExample21[nameof(IpAddress).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureUserExample2,
        };
        public static OpenApiObject SessionExample22 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureSessionExample22[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = PureSessionExample22[nameof(UserId).ToSnakeCase()],
            [nameof(IpAddress).ToSnakeCase()] = PureSessionExample22[nameof(IpAddress).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureUserExample2,
        };
        public static OpenApiArray ListOfSessionsExample1 { get; } = new OpenApiArray
        {
            SessionExample11, SessionExample12
        };
        public static OpenApiArray ListOfSessionsExample2 { get; } = new OpenApiArray
        {
            SessionExample21, SessionExample22
        };

        public static OpenApiObject Example { get; } = SessionExample11;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(SessionController.GetSessionByIdAsync)] = SessionExample11,
            [nameof(SessionController.ListSessionsAsync)] = ListOfPureSessionsExample1,
        };

        static SessionDto()
        {
            PureSessionExample11[nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample1[nameof(UserDto.Id).ToSnakeCase()];
            PureSessionExample12[nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample1[nameof(UserDto.Id).ToSnakeCase()];
            PureSessionExample21[nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample2[nameof(UserDto.Id).ToSnakeCase()];
            PureSessionExample22[nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample2[nameof(UserDto.Id).ToSnakeCase()];
        }

        public SessionDto(long? id, long userId, string ipAddress, UserDto user)
        {
            Id = id;
            UserId = userId;
            IpAddress = ipAddress;
            User = user;
        }
    }
}
