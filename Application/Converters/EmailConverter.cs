using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;

namespace FireplaceApi.Application.Converters;

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
