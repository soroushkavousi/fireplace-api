using Newtonsoft.Json;

namespace FireplaceApi.Domain.Tools.NewtonsoftSerializer;

public class CoreJsonSerializerSettings : JsonSerializerSettings
{
    public static readonly CoreJsonSerializerSettings Instance = new();

    protected CoreJsonSerializerSettings() : base()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        ContractResolver = CoreContractResolver.Instance;
        Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        Converters.Add(new IPAddressConverter());
        Converters.Add(new IPEndPointConverter());
    }
}
