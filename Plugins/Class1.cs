using PluginSystem;
[PluginLoad]
public class HelloPlugin : IPluginCommand
{
    public void Execute()
    {
        Console.WriteLine("Hello from HelloPlugin!");
    }
}

[PluginLoad]
public class CalculatorPlugin : IPluginCommand
{
    public void Execute()
    {
        Console.WriteLine("Calculator plugin: 2 + 2 = 4");
    }
}