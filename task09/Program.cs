using System;
using System.Reflection;
using System.Linq;

namespace ConsoleApp
{
    public class ConsolePrinter
    {
        public static void PrintConstructorInfo(ConstructorInfo constructor)
        {
            Console.WriteLine($"Имя конструктора: {constructor.Name}");

            var paramsOfConstructor = constructor.GetParameters();

            Console.WriteLine(paramsOfConstructor.Any() ?
                string.Join("\n", paramsOfConstructor.Select(param => $"  Имя параметра - {param.Name}, тип параметра - {param.ParameterType}")) :
                "  Параметры отсутствуют");

            Console.WriteLine();
        }

        public static void PrintMethodInfo(MethodInfo method)
        {
            var displayNameAttr = method.GetCustomAttribute<DisplayNameAttribute>();
            Console.WriteLine($"Имя метода: {method.Name}" + 
                (displayNameAttr != null ? $" (Отображаемое имя: {displayNameAttr.DisplayName})" : ""));

            var paramsOfMethod = method.GetParameters();

            Console.WriteLine(paramsOfMethod.Any() ?
                string.Join("\n", paramsOfMethod.Select(param => $"  Имя параметра - {param.Name}, тип параметра - {param.ParameterType}")) :
                "  Параметры отсутствуют");

            Console.WriteLine();
        }

        public static void PrintTypeInfo(Type type)
        {
            var versionAttr = type.GetCustomAttribute<VersionAttribute>();
            var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();

            Console.WriteLine($"Имя класса: {type.FullName}");
            if (displayNameAttr != null)
                Console.WriteLine($"Отображаемое имя: {displayNameAttr.DisplayName}");
            if (versionAttr != null)
                Console.WriteLine($"Версия: {versionAttr.Major}.{versionAttr.Minor}");
            Console.WriteLine();
            var properties = type.GetProperties();
            Console.WriteLine(properties.Any() ? "Свойства:" : "Свойства отсутствуют");
            foreach (var prop in properties)
            {
                var propDisplayName = prop.GetCustomAttribute<DisplayNameAttribute>();
                Console.WriteLine($"- {prop.PropertyType.Name} {prop.Name}" + 
                    (propDisplayName != null ? $" (Отображаемое имя: {propDisplayName.DisplayName})" : ""));
            }
            Console.WriteLine();
            var methodsOfType = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | 
                                             BindingFlags.Public | BindingFlags.DeclaredOnly)
                                 .Where(m => !m.IsSpecialName);
            Console.WriteLine(methodsOfType.Any() ? "Методы:" : "Методы отсутствуют");
            foreach (var method in methodsOfType) 
                PrintMethodInfo(method);
            var constructorsOfType = type.GetConstructors();
            Console.WriteLine(constructorsOfType.Any() ? "Конструкторы:" : "Конструкторы отсутствуют");
            foreach (var constructor in constructorsOfType) 
                PrintConstructorInfo(constructor);
            var otherAttributes = type.GetCustomAttributes(true)
                                    .Where(a => !(a is DisplayNameAttribute) && !(a is VersionAttribute))
                                    .ToArray();

            Console.WriteLine(otherAttributes.Any() ? "Другие атрибуты:" : "Другие атрибуты отсутствуют");
            foreach (var attribute in otherAttributes)
                Console.WriteLine($"- {attribute.GetType().Name}");
            
            Console.WriteLine(new string('-', 50));
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Укажите путь к библиотеке в аргументах командной строки");
                return;
            }

            try
            {
                string libraryPath = args[0];
                Assembly assembly = Assembly.LoadFrom(libraryPath);
                var classesOfLibrary = assembly.GetTypes().Where(t => t.IsClass);

                foreach (var type in classesOfLibrary)
                {
                    PrintTypeInfo(type);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

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
}