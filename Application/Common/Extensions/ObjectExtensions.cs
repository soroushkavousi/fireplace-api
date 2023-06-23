namespace FireplaceApi.Application.Common;

public static class ObjectExtensions
{
    public static DestinationType To<DestinationType>(this object obj)
    {
        if (obj == null)
            return default;
        return (DestinationType)obj;
    }
}
