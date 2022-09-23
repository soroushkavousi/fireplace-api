namespace FireplaceApi.Core.Tools.NewtonsoftSerializer
{
    public class SensitiveJsonSerializerSettings : CoreJsonSerializerSettings
    {
        public static new readonly SensitiveJsonSerializerSettings Instance = new();

        public SensitiveJsonSerializerSettings() : base()
        {
            ContractResolver = SensitiveContractResolver.Instance;
        }
    }
}
