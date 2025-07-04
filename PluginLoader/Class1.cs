using System;
using System.IO;
using System.Reflection;
namespace PluginSystem
{
[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute : Attribute { }

public interface IPluginCommand
{
    void Execute();
}

public class PluginLoader
{
    public void LoadAndExecutePlugins(string pluginsPath)
    {
        if (!Directory.Exists(pluginsPath))
        {
            Directory.CreateDirectory(pluginsPath);
            Console.WriteLine($"Created plugins directory: {pluginsPath}");
            return;
        }

        foreach (var dllPath in Directory.GetFiles(pluginsPath, "*.dll"))
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<PluginLoadAttribute>() != null && 
                        typeof(IPluginCommand).IsAssignableFrom(type))
                    {
                        try
                        {
                            IPluginCommand plugin = (IPluginCommand)Activator.CreateInstance(type);
                            plugin.Execute();
                            Console.WriteLine($"Executed plugin: {type.Name}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to execute {type.Name}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load {Path.GetFileName(dllPath)}: {ex.Message}");
            }
        }
    }
}
}