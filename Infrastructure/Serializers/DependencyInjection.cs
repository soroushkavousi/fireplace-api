using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Infrastructure.Serializers;

public static class DependencyInjection
{
    public static void AddSerializers(this IServiceCollection services)
    {
        services.AddSingleton<ISerializer, Serializer>();
    }
}
