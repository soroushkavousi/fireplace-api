namespace FireplaceApi.Application.Common;

public interface ISerializer
{
    public void Serialize<T>(T data, bool ignoreSensitiveLimit = false);
    public T Deserialize<T>(string serializedData);
}
