using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Errors;

namespace FireplaceApi.Presentation.Dtos;

public record CommunityEncodedIdOrName(string Value)
{
    private readonly CommunityIdentifier _communityIdentifier = DecodeValue(Value);

    public CommunityIdentifier CommunityIdentifier => _communityIdentifier;

    private static CommunityIdentifier DecodeValue(string value)
    {
        try
        {
            var id = value.IdDecode();
            return CommunityIdentifier.OfId(id);
        }
        catch (ApiException) { }

        try
        {
            var name = new CommunityName(value);
            return CommunityIdentifier.OfName(name);
        }
        catch (ApiException) { }

        throw new CommunityEncodedIdOrNameInvalidFormatException(value);
    }
}

public static class CommunityEncodedIdOrNameExtensions
{
    public static CommunityIdentifier ToCommunityIdentifier(this string communityIdOrName)
        => new CommunityEncodedIdOrName(communityIdOrName).CommunityIdentifier;
}
