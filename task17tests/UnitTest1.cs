using Xunit;
using System;
using System.Threading;
using Command;
using task17; 

public class ServerThreadTests
{
    [Fact]
    public void HardStop_ImmediatelyStopsThread()
    {
       
        var server = new ServerThread();
        var testCmd = new TestCommand();
        
        server.Start();
        server.AddCommand(testCmd);
        server.AddCommand(new HardStopCommand(server));
        server.AddCommand(testCmd);
        
        Thread.Sleep(100);
        
        Assert.Equal(1, testCmd.ExecutionCount);
    }

    [Fact]
    public void SoftStop_WaitsForQueueToEmpty()
    {
        var server = new ServerThread();
        var testCmd = new TestCommand();
        
        server.Start();
        server.AddCommand(testCmd);
        server.AddCommand(new SoftStopCommand(server));
        server.AddCommand(testCmd); 
        
        Thread.Sleep(150);
        
        Assert.Equal(2, testCmd.ExecutionCount);
    }

    [Fact]
    public void Commands_ExecuteInOrder()
    {
        var server = new ServerThread();
        var cmd1 = new TestCommand();
        var cmd2 = new TestCommand();

        server.Start();
        server.AddCommand(cmd1);
        server.AddCommand(cmd2);
        
        Thread.Sleep(100);
        
        Assert.Equal(1, cmd1.ExecutionCount);
        Assert.Equal(1, cmd2.ExecutionCount);
    }

    private class TestCommand : ICommand
    {
        public int ExecutionCount { get; private set; }
        
        public void Execute() 
        {
            ExecutionCount++;
            Thread.Sleep(20);
        }
    }
}