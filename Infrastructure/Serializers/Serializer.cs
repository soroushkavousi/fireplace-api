namespace FireplaceApi.Infrastructure.Serializers;

public class Serializer : ISerializer
{
    public void Serialize<T>(T data, bool ignoreSensitiveLimit = false)
        => data.ToJson(ignoreSensitiveLimit);

    public T Deserialize<T>(string serializedData)
        => serializedData.FromJson<T>();
}
