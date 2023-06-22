using Newtonsoft.Json;

namespace FireplaceApi.Application.Common;

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
