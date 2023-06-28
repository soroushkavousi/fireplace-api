using FireplaceApi.Domain.Comments;
using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Posts;
using FireplaceApi.Infrastructure.Errors;

namespace FireplaceApi.Infrastructure.ValueObjects;

public record EnumString<TEnum>(string Value) where TEnum : struct
{
    private readonly TEnum _enum = ConvertValueToEnum(Value);

    public TEnum Enum => _enum;

    private static TEnum ConvertValueToEnum(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !System.Enum.TryParse(value, true, out TEnum result))
        {
            throw typeof(TEnum).Name switch
            {
                nameof(CommunitySortType) => new CommunitySortIncorrectValueException(value),
                nameof(PostSortType) => new PostSortIncorrectValueException(value),
                nameof(CommentSortType) => new CommentSortIncorrectValueException(value),
                _ => new InternalServerException("Not known enum type!"),
            };
        }

        return result;
    }
}

public static class EnumStringExtensions
{
    public static TEnum ToEnum<TEnum>(this string enumString) where TEnum : struct
        => new EnumString<TEnum>(enumString).Enum;

    public static TEnum? ToNullableEnum<TEnum>(this string enumString) where TEnum : struct
    {
        try
        {
            return enumString.ToEnum<TEnum>();
        }
        catch (ApiException)
        {
            return null;
        }
    }
}
