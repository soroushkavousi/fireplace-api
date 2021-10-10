using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Api.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class EmailDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string ActivationStatus { get; set; }
        public UserDto User { get; set; }

        public static OpenApiObject PureExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(10001),
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(Address).ToSnakeCase()] = new OpenApiString("tedmosby@gmail.com"),
            [nameof(ActivationStatus).ToSnakeCase()] = new OpenApiString(Core.Enums.ActivationStatus.COMPLETED.ToString()),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(20001),
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(Address).ToSnakeCase()] = new OpenApiString("barneystinson@gmail.com"),
            [nameof(ActivationStatus).ToSnakeCase()] = new OpenApiString(Core.Enums.ActivationStatus.SENT.ToString()),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiArray PureListExample1 { get; } = new OpenApiArray
        {
            PureExample1, PureExample2
        };

        public static OpenApiObject Example1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample1[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(Address).ToSnakeCase()] = PureExample1[nameof(Address).ToSnakeCase()],
            [nameof(ActivationStatus).ToSnakeCase()] = PureExample1[nameof(ActivationStatus).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureExample1,
        };
        public static OpenApiObject Example2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(Address).ToSnakeCase()] = PureExample2[nameof(Address).ToSnakeCase()],
            [nameof(ActivationStatus).ToSnakeCase()] = PureExample2[nameof(ActivationStatus).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureExample2,
        };

        public static OpenApiObject Example { get; } = Example1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(EmailController.ActivateEmail)] = Example1,
            [nameof(EmailController.GetEmailByIdAsync)] = Example1
        };

        static EmailDto()
        {
            PureExample1[nameof(UserId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()];
            PureExample2[nameof(UserId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()];
        }

        public EmailDto(long id, long userId, string address,
            string activationStatus, UserDto user = null)
        {
            Id = id;
            UserId = userId;
            Address = address;
            ActivationStatus = activationStatus;
            User = user;
        }
    }
}
