using Xunit;
using Moq;
using System;
using task04; 

public class SpaceshipTests
{
    // Тесты для крейсера
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Cruiser_MoveForward_ShouldPrintCorrectMessage()
    {
        var cruiser = new Cruiser();
        var consoleOutput = new ConsoleOutput();
        
        cruiser.MoveForward();
        
        Assert.Equal($"Крейсер движется вперед со скоростью {cruiser.Speed} единиц{Environment.NewLine}", 
                    consoleOutput.GetOutput());
    }

    [Fact]
    public void Cruiser_Rotate_ShouldPrintCorrectMessage()
    {
        var cruiser = new Cruiser();
        var consoleOutput = new ConsoleOutput();
        int testAngle = 30;
        
        cruiser.Rotate(testAngle);
        
        Assert.Equal($"Крейсер поворачивается на {testAngle} градусов (медленный разворот){Environment.NewLine}", 
                    consoleOutput.GetOutput());
    }

    [Fact]
    public void Cruiser_Fire_ShouldPrintCorrectMessage()
    {
        var cruiser = new Cruiser();
        var consoleOutput = new ConsoleOutput();
        
        cruiser.Fire();
        
        Assert.Equal($"Крейсер стреляет ракетой! Урон: {cruiser.FirePower}{Environment.NewLine}", 
                    consoleOutput.GetOutput());
    }

    // Тесты для истребителя
    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(150, fighter.Speed);  
        Assert.Equal(30, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Fighter_MoveForward_ShouldPrintCorrectMessage()
    {
        var fighter = new Fighter();
        var consoleOutput = new ConsoleOutput();
        
        fighter.MoveForward();
        
        Assert.Equal($"Истребитель летит со скоростью {fighter.Speed} единиц{Environment.NewLine}", 
                    consoleOutput.GetOutput());
    }

    [Fact]
    public void Fighter_Rotate_ShouldPrintCorrectMessage()
    {
        var fighter = new Fighter();
        var consoleOutput = new ConsoleOutput();
        int testAngle = 45;
        
        fighter.Rotate(testAngle);
        
        Assert.Equal($"Истребитель быстро разворачивается на {testAngle} градусов{Environment.NewLine}", 
                    consoleOutput.GetOutput());
    }

    [Fact]
    public void Fighter_Fire_ShouldPrintCorrectMessage()
    {
        var fighter = new Fighter();
        var consoleOutput = new ConsoleOutput();
        
        fighter.Fire();
        
        Assert.Equal($"Истребитель выпускает ракеты! Урон каждой: {fighter.FirePower}{Environment.NewLine}", 
                    consoleOutput.GetOutput());
    }
}

// Вспомогательный класс для перехвата вывода в консоль
public class ConsoleOutput : IDisposable
{
    private readonly System.IO.StringWriter stringWriter;
    private readonly System.IO.TextWriter originalOutput;

    public ConsoleOutput()
    {
        stringWriter = new System.IO.StringWriter();
        originalOutput = Console.Out;
        Console.SetOut(stringWriter);
    }

    public string GetOutput()
    {
        return stringWriter.ToString();
    }

    public void Dispose()
    {
        Console.SetOut(originalOutput);
        stringWriter.Dispose();
    }
}