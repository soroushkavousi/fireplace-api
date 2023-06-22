using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Application.Common;

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
