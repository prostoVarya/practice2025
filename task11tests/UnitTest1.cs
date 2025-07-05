using System;
using Xunit;

public class CalculatorTests
{
    [Fact]
    public void TestAddition()
    {
        dynamic calculator = CreateCalculator();
        Assert.Equal(8, calculator.Add(5, 3));
    }
    
    [Fact]
    public void TestSubtraction()
    {
        dynamic calculator = CreateCalculator();
        Assert.Equal(2, calculator.Minus(5, 3));
    }
    
    [Fact]
    public void TestMultiplication()
    {
        dynamic calculator = CreateCalculator();
        Assert.Equal(15, calculator.Mul(5, 3));
    }
    
    [Fact]
    public void TestDivision()
    {
        dynamic calculator = CreateCalculator();
        Assert.Equal(2, calculator.Div(6, 3));
    }
    
    [Fact]
    public void TestDivisionByZero()
    {
        dynamic calculator = CreateCalculator();
        Assert.Throws<DivideByZeroException>(() => calculator.Div(5, 0));
    }
    
    private static object CreateCalculator()
    {
        Type calculatorType = Program.CreateCalculatorType();
        return Activator.CreateInstance(calculatorType);
    }
}
