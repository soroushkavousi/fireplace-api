namespace FireplaceApi.Infrastructure.Entities;

public abstract class ServerIdentifier
{
    public static ServerIdIdentifier OfId(ulong id)
        => new(id);

    public static ServerMacAddressIdentifier OfMacAddress(string macAddress)
        => new(macAddress);
}

public class ServerIdIdentifier : ServerIdentifier, IIdIdentifier
{
    public ulong Id { get; set; }

    internal ServerIdIdentifier(ulong id)
    {
        Id = id;
    }
}

public class ServerMacAddressIdentifier : ServerIdentifier
{
    public string MacAddress { get; set; }

    internal ServerMacAddressIdentifier(string macAddress)
    {
        MacAddress = macAddress;
    }
}
