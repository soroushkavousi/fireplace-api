using FireplaceApi.Domain.Attributes;
using FireplaceApi.Domain.Extensions;

namespace FireplaceApi.Domain.ValueObjects;

public class Password
{
    private string _hash;
    [Sensitive]
    public string Value { get; set; }
    public string Hash { get => _hash ?? ComputeHashOfPasswordValue(Value); set => _hash = value; }

    private Password(string value = null, string hash = null)
    {
        Value = value;
        Hash = hash;
    }

    public static Password OfValue(string passwordValue)
    {
        if (passwordValue == null)
            return null;
        return new Password(value: passwordValue);
    }

    public static Password OfHash(string passwordHash)
    {
        if (passwordHash == null)
            return null;
        return new Password(hash: passwordHash);
    }

    private string ComputeHashOfPasswordValue(string password)
    {
        _hash = password.ToHash();
        return _hash;
    }
}
