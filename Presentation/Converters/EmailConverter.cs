using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.ValueObjects;

namespace FireplaceApi.Presentation.Converters;

public static class EmailConverter
{
    public static EmailDto ToDto(this Email email)
    {
        if (email == null)
            return null;

        var emailDto = new EmailDto(email.Id.IdEncode(), email.UserId.IdEncode(), email.Address,
            email.Activation.Status.ToString());

        return emailDto;
    }

    public static QueryResultDto<EmailDto> ToDto(this QueryResult<Email> queryResult)
        => queryResult.ToDto(ToDto);
}
