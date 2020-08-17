using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ReflectionHelper
{
    public const BindingFlags HierarchyBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

    private static readonly string[] BlackList = { "Microsoft.VisualStudio.TraceDataCollector,", "protobuf-net", "NetFabric.Hyperlinq" };

    private static MethodInfo[] cachedExtensionMethods;
    
    static ReflectionHelper()
    {
        // precache all extension methods
        CacheExtensionMethods();
    }
    
    /// <summary> Returns all derived types for <paramref name="baseType"/> optionally in specific <paramref name="assembly"/> and optionally (if <paramref name="includeSelf"/> set) returning the type itself. </summary>
    public static IEnumerable<Type> DerivedTypes(this Type baseType, Assembly assembly = null, bool includeSelf = false)
        => assembly != null ? assembly.DerivedTypes(baseType, includeSelf) : GetFilteredAssemblies().DerivedTypes(baseType, includeSelf);
    
    /// <summary> Returns all derived types for <paramref name="baseType"/> in <paramref name="assemblies"/> optionally (if <paramref name="includeSelf"/> set) returning the type itself. </summary>
    public static IEnumerable<Type> DerivedTypes(this IEnumerable<Assembly> assemblies, Type baseType, bool includeSelf = false) =>
        assemblies.SelectMany(a => a.DerivedTypes(baseType, includeSelf));

    /// <summary> Returns all derived types for <paramref name="baseType"/> in <paramref name="assembly"/> optionally (if <paramref name="includeSelf"/> set) returning the type itself. </summary>
    private static IEnumerable<Type> DerivedTypes(this Assembly assembly, Type baseType, bool includeSelf = false)
    {
        try
        {
            return assembly.GetTypes().Where(t => (includeSelf || t != baseType) && baseType.IsAssignableFrom(t));
        }
        catch (Exception)
        {
            // Failed to get derived types for '{baseType.FullName}' in assembly '{assembly.FullName}' (usually safe to ignore for third party assembly)
            return Array.Empty<Type>();
        }
    }
    
    static void CacheExtensionMethods()
    {
        var assemblyTypes = GetFilteredAssemblyTypes();

        // this is fairly slow, hence the pre-cached list of all extension methods
        var allExtensionMethods = from t in assemblyTypes
            where t.IsSealed && !t.IsGenericType && !t.IsNested
            from method in t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            where method.IsDefined(typeof(ExtensionAttribute), false)
            select method;

        cachedExtensionMethods = allExtensionMethods.ToArray();
    }
    
    public static IEnumerable<MethodInfo> MethodsWithAttribute<T>(this Type type) where T : Attribute
    {
        var methods = type.GetMethods(BindingFlags.Static | HierarchyBinding);
        return methods
            .Union(type.GetExtensionMethods())
            .Where(m => m.Attribute<T>() != null);
    }

    public static T Attribute<T>(this MemberInfo member, bool inherit = true) where T : Attribute => System.Attribute.GetCustomAttributes(member, typeof(T), inherit).FirstOrDefault() as T;
    
    public static IEnumerable<Assembly> FilteredAssemblies(this IEnumerable<Assembly> assemblies) => assemblies.Where(a => !BlackList.Any(blacklistedAssemblyName => a.FullName.StartsWith(blacklistedAssemblyName)));

    public static IEnumerable<Assembly> GetFilteredAssemblies() => AppDomain.CurrentDomain.GetAssemblies().FilteredAssemblies();
    public static IEnumerable<Type> GetFilteredAssemblyTypes()  => GetFilteredAssemblies().SelectMany(a => a.GetTypes());


    public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type)
    {
        var methods = from method in cachedExtensionMethods
            where method.GetParameters()[0].ParameterType.IsAssignableFrom(type)
            select method;

        return methods;
    }
}
