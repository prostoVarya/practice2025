using System;
using System.Reflection;
namespace task07{

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public sealed class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }
    public DisplayNameAttribute(string displayName) => DisplayName = displayName;
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }
    public VersionAttribute(int major, int minor) => (Major, Minor) = (major, minor);
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public sealed class SampleClass
{
    [DisplayName("Числовое свойство")]
    public int Number { get; set; }

    [DisplayName("Тестовый метод")]
    public void TestMethod() { }
}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (GetAttribute<DisplayNameAttribute>(type) is { } displayName)
            Console.WriteLine($"Отображаемое имя класса: {displayName.DisplayName}");

        if (GetAttribute<VersionAttribute>(type) is { } version)
            Console.WriteLine($"Версия класса: {version.Major}.{version.Minor}");

        Console.WriteLine("\nСвойства:");
        foreach (var prop in type.GetProperties())
        {
            var display = GetAttribute<DisplayNameAttribute>(prop)?.DisplayName ?? "нет атрибута DisplayName";
            Console.WriteLine($"- {prop.Name}: {display}");
        }

        Console.WriteLine("\nМетоды:");
        foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            var display = GetAttribute<DisplayNameAttribute>(method)?.DisplayName ?? "нет атрибута DisplayName";
            Console.WriteLine($"- {method.Name}: {display}");
        }
    }

    private static T? GetAttribute<T>(MemberInfo member) where T : Attribute
        => Attribute.GetCustomAttribute(member, typeof(T)) as T;
}
}