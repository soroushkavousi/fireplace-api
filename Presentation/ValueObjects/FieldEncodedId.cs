using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Errors;

namespace FireplaceApi.Presentation.ValueObjects;

public partial record FieldEncodedId
{
    public EncodedId EncodedId { get; init; }
    public FieldName FieldName { get; init; }
    public ulong Id => EncodedId.Id;

    public FieldEncodedId(string value, FieldName field)
    {
        FieldName = field;
        try
        {
            EncodedId = new EncodedId(value);
        }
        catch
        {
            throw field.Name switch
            {
                nameof(FieldName.COMMUNITY_ID) => new CommunityEncodedIdInvalidFormatException(value),
                nameof(FieldName.POST_ID) => new PostEncodedIdInvalidFormatException(value),
                nameof(FieldName.COMMENT_ID) => new CommentEncodedIdInvalidFormatException(value),
                _ => new InternalServerException("Not known encoded id field!"),
            };
        }
    }
}

public static class FieldEncodedIdExtensions
{
    public static ulong ToId(this string value, FieldName field)
        => new FieldEncodedId(value, field).Id;
}

