using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FireplaceApi.Infrastructure.Tools;

public static class Utils
{
    public static List<Type> FindChildTypes<T>(bool isLocal = true) where T : class
    {
        return Assembly.GetAssembly(typeof(T)).GetTypes().Where(type =>
            type.IsClass
            && !type.IsAbstract
            && !type.IsInterface
            && type.IsAssignableTo(typeof(T))
            && !(isLocal && !type.IsLocal())
            ).ToList();
    }

    public static bool IsLocal(this Type type)
        => type.Namespace != null && type.Namespace.StartsWith(nameof(FireplaceApi));
}
