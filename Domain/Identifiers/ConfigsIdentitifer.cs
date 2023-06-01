using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Interfaces;

namespace FireplaceApi.Domain.Identifiers;

public abstract class ConfigsIdentifier
{
    public static ConfigsIdIdentifier OfId(ulong id)
        => new(id);

    public static ConfigsEnvironmentNameIdentifier OfEnvironmentName(EnvironmentName environmentName)
        => new(environmentName);
}

public class ConfigsIdIdentifier : ConfigsIdentifier, IIdIdentifier
{
    public ulong Id { get; set; }

    internal ConfigsIdIdentifier(ulong id)
    {
        Id = id;
    }
}

public class ConfigsEnvironmentNameIdentifier : ConfigsIdentifier
{
    public EnvironmentName EnvironmentName { get; set; }

    internal ConfigsEnvironmentNameIdentifier(EnvironmentName environmentName)
    {
        EnvironmentName = environmentName;
    }
}
