using PluginSystem;
class Program
{
    static void Main()
    {
        string pluginsPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        var loader = new PluginLoader();
        
        Console.WriteLine("Loading plugins...");
        loader.LoadAndExecutePlugins(pluginsPath);
        
        Console.WriteLine("All plugins executed!");
    }
}
