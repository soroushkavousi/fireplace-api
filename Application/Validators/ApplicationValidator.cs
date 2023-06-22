using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Extensions;

namespace FireplaceApi.Application.Validators;

public class ApplicationValidator
{
    public void ValidateUrlStringFormat(string urlString, FieldName field)
    {
        if (!urlString.IsUrlStringValid())
        {
            throw field.Name switch
            {
                nameof(FieldName.AVATART_URL) => throw new AvatarUrlInvalidFormatException(urlString),
                nameof(FieldName.BANNER_URL) => throw new BannerUrlInvalidFormatException(urlString),
                _ => throw new InternalServerException("Not known url!")
            };
        }
    }
}
