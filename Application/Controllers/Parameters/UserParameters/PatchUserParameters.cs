using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Attributes;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers
{
    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class SendResetPasswordCodeInputBodyParameters
    {
        [Required]
        public string EmailAddress { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureExample1[nameof(EmailDto.Address).ToSnakeCase()],
        };
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class ResetPasswordWithCodeInputBodyParameters
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string ResetPasswordCode { get; set; }
        [Required]
        public string NewPassword { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(EmailAddress).ToSnakeCase()] = EmailDto.PureExample1[nameof(EmailDto.Address).ToSnakeCase()],
            [nameof(ResetPasswordCode).ToSnakeCase()] = new OpenApiString("6Qw2RsG8aw"),
            [nameof(NewPassword).ToSnakeCase()] = new OpenApiString("NewPassword"),
        };
    }

    [SwaggerSchemaFilter(typeof(TypeExampleProvider))]
    public class PatchUserInputBodyParameters
    {
        public string DisplayName { get; set; }
        public string About { get; set; }
        public string AvatarUrl { get; set; }
        public string BannerUrl { get; set; }
        public string Username { get; set; }
        [Sensitive]
        public string OldPassword { get; set; }
        [Sensitive]
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public static IOpenApiAny Example { get; } = new OpenApiObject
        {
            [nameof(DisplayName).ToSnakeCase()] = new OpenApiString("NewDisplayName"),
            [nameof(About).ToSnakeCase()] = new OpenApiString("NewAbout"),
            [nameof(AvatarUrl).ToSnakeCase()] = new OpenApiString("NewAvatarUrl"),
            [nameof(BannerUrl).ToSnakeCase()] = new OpenApiString("NewBannerUrl"),
            [nameof(Username).ToSnakeCase()] = new OpenApiString("NewUsername"),
            [nameof(OldPassword).ToSnakeCase()] = new OpenApiString("OldPassword"),
            [nameof(Password).ToSnakeCase()] = new OpenApiString("NewPassword"),
            [nameof(EmailAddress).ToSnakeCase()] = new OpenApiString("NewEmailAddress"),
        };
    }
}
