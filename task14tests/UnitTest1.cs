using Xunit;
using task14;

public class DefiniteIntegralTests
{
    [Fact]
    public void Solve_LinearFunction_SymmetricInterval_ReturnsZero()
    {
        var linear = (double x) => x;
        double result = DefiniteIntegral.Solve(-1, 1, linear, 1e-4, 2);
        Assert.Equal(0, result, 4);  
    }

    [Fact]
    public void Solve_SineFunction_SymmetricInterval_ReturnsZero()
    {
        var sine = (double x) => Math.Sin(x);
        double result = DefiniteIntegral.Solve(-1, 1, sine, 1e-5, 8);
        Assert.Equal(0, result, 4);  
    }

    [Fact]
    public void Solve_LinearFunction_PositiveInterval_ReturnsCorrectArea()
    {
        var linear = (double x) => x;
        double result = DefiniteIntegral.Solve(0, 5, linear, 1e-6, 8);
        Assert.Equal(12.5, result, 5);  
    }

    [Fact]
    public void Solve_ConstantFunction_ReturnsCorrectArea()
    {
        var constant = (double x) => 2.5;
        double result = DefiniteIntegral.Solve(0, 10, constant, 1e-4, 4);
        Assert.Equal(25, result, 4);
    }

    [Fact]
    public void Solve_QuadraticFunction_ReturnsCorrectArea()
    {
        var quadratic = (double x) => x * x;
        double result = DefiniteIntegral.Solve(0, 3, quadratic, 1e-5, 8);
        Assert.Equal(9, result, 4);
    }

    [Fact]
    public void Solve_WithDifferentThreadCounts_ReturnsConsistentResults()
    {
        var func = (double x) => Math.Exp(-x * x);
        double result1 = DefiniteIntegral.Solve(0, 1, func, 1e-5, 1);
        double result2 = DefiniteIntegral.Solve(0, 1, func, 1e-5, 4);
        double result3 = DefiniteIntegral.Solve(0, 1, func, 1e-5, 8);
        Assert.Equal(result1, result2, 5);
        Assert.Equal(result2, result3, 5);
    }
}