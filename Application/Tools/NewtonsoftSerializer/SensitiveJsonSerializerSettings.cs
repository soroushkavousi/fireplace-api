namespace FireplaceApi.Application.Tools.NewtonsoftSerializer;

public class SensitiveJsonSerializerSettings : CoreJsonSerializerSettings
{
    public static new readonly SensitiveJsonSerializerSettings Instance = new();

    protected SensitiveJsonSerializerSettings() : base()
    {
        ContractResolver = SensitiveContractResolver.Instance;
    }
}
