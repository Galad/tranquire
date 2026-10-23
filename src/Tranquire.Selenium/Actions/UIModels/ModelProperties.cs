using System;
using System.Linq;
using System.Reflection;

namespace Tranquire.Selenium.Actions.UIModels;

internal static class ModelProperties
{
    // Readable public properties, without indexers or base properties hidden with 'new'.
    public static PropertyInfo[] Of(Type type)
    {
        return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanRead && property.GetIndexParameters().Length == 0)
            .GroupBy(property => property.Name, StringComparer.Ordinal)
            .Select(group => group.OrderByDescending(property => InheritanceDepth(property.DeclaringType)).First())
            .ToArray();
    }

    private static int InheritanceDepth(Type type)
    {
        var depth = 0;
        for (var current = type.BaseType; current != null; current = current.BaseType)
            depth++;
        return depth;
    }
}
