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

namespace FireplaceApi.Api.Controllers.Parameters.EmailParameters
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class EmailDto
    {
        [Required]
        public long? Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string ActivationStatus { get; set; }
        public UserDto User { get; set; }

        public static OpenApiObject PureEmailExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(1000001),
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(Address).ToSnakeCase()] = new OpenApiString("tedmosby@gmail.com"),
            [nameof(ActivationStatus).ToSnakeCase()] = new OpenApiString(Core.Enums.ActivationStatus.COMPLETED.ToString()),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiObject PureEmailExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = new OpenApiInteger(2000001),
            [nameof(UserId).ToSnakeCase()] = null,
            [nameof(Address).ToSnakeCase()] = new OpenApiString("barneystinson@gmail.com"),
            [nameof(ActivationStatus).ToSnakeCase()] = new OpenApiString(Core.Enums.ActivationStatus.SENT.ToString()),
            [nameof(User).ToSnakeCase()] = new OpenApiNull(),
        };
        public static OpenApiArray ListOfPureEmailsExample1 { get; } = new OpenApiArray
        {
            PureEmailExample1, PureEmailExample2
        };

        public static OpenApiObject EmailExample1 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureEmailExample1[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample1[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(Address).ToSnakeCase()] = PureEmailExample1[nameof(Address).ToSnakeCase()],
            [nameof(ActivationStatus).ToSnakeCase()] = PureEmailExample1[nameof(ActivationStatus).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureUserExample1,
        };
        public static OpenApiObject EmailExample2 { get; } = new OpenApiObject
        {
            [nameof(Id).ToSnakeCase()] = PureEmailExample2[nameof(Id).ToSnakeCase()],
            [nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample2[nameof(UserDto.Id).ToSnakeCase()],
            [nameof(Address).ToSnakeCase()] = PureEmailExample2[nameof(Address).ToSnakeCase()],
            [nameof(ActivationStatus).ToSnakeCase()] = PureEmailExample2[nameof(ActivationStatus).ToSnakeCase()],
            [nameof(User).ToSnakeCase()] = UserDto.PureUserExample2,
        };

        public static OpenApiObject Example { get; } = EmailExample1;
        public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
        {
            [nameof(EmailController.ActivateEmail)] = EmailExample1,
            [nameof(EmailController.GetEmailByIdAsync)] = EmailExample1
        };

        static EmailDto()
        {
            PureEmailExample1[nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample1[nameof(UserDto.Id).ToSnakeCase()];
            PureEmailExample2[nameof(UserId).ToSnakeCase()] = UserDto.PureUserExample2[nameof(UserDto.Id).ToSnakeCase()];
        }

        public EmailDto(long? id, long userId, string address, 
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
