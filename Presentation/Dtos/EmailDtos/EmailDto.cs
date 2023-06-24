using FireplaceApi.Presentation.Controllers;
using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Presentation.Swagger;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

[SwaggerSchemaFilter(typeof(TypeExampleProvider))]
public class EmailDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string ActivationStatus { get; set; }

    public static OpenApiObject PureExample1 { get; } = new OpenApiObject
    {
        [nameof(Id).ToSnakeCase()] = new OpenApiString("TebvzX5eHJN"),
        [nameof(UserId).ToSnakeCase()] = null,
        [nameof(Address).ToSnakeCase()] = new OpenApiString("erenyeager@gmail.com"),
        [nameof(ActivationStatus).ToSnakeCase()] = new OpenApiString(Domain.Emails.ActivationStatus.COMPLETED.ToString()),
    };
    public static OpenApiObject PureExample2 { get; } = new OpenApiObject
    {
        [nameof(Id).ToSnakeCase()] = new OpenApiString("SFhCNah9niG"),
        [nameof(UserId).ToSnakeCase()] = null,
        [nameof(Address).ToSnakeCase()] = new OpenApiString("lelouchlamperouge@gmail.com"),
        [nameof(ActivationStatus).ToSnakeCase()] = new OpenApiString(Domain.Emails.ActivationStatus.SENT.ToString()),
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
    };
    public static OpenApiObject Example2 { get; } = new OpenApiObject
    {
        [nameof(Id).ToSnakeCase()] = PureExample2[nameof(Id).ToSnakeCase()],
        [nameof(UserId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()],
        [nameof(Address).ToSnakeCase()] = PureExample2[nameof(Address).ToSnakeCase()],
        [nameof(ActivationStatus).ToSnakeCase()] = PureExample2[nameof(ActivationStatus).ToSnakeCase()],
    };

    public static OpenApiObject Example { get; } = Example1;
    public static Dictionary<string, IOpenApiAny> ActionExamples { get; } = new Dictionary<string, IOpenApiAny>
    {
        [nameof(EmailController.ActivateRequestingUserEmailAsync)] = Example1,
        [nameof(EmailController.ResendActivationCodeAsync)] = new OpenApiNull(),
        [nameof(EmailController.GetRequestingUserEmailAsync)] = Example1,
        [nameof(EmailController.PatchEmailAsync)] = Example1
    };

    static EmailDto()
    {
        PureExample1[nameof(UserId).ToSnakeCase()] = UserDto.PureExample1[nameof(UserDto.Id).ToSnakeCase()];
        PureExample2[nameof(UserId).ToSnakeCase()] = UserDto.PureExample2[nameof(UserDto.Id).ToSnakeCase()];
    }

    public EmailDto(string id, string userId, string address,
        string activationStatus)
    {
        Id = id;
        UserId = userId;
        Address = address;
        ActivationStatus = activationStatus;
    }
}
