using ServerThreadSystem;
namespace task18tests
{
    public class ServerThreadSystemTests
    {
        public class TestLongCommand : IServerThreadLongCommand
        {
            public ServerThread ServerThread { get; }
            public int CurrCount { get; private set; }

            public int NumberOfExecutions { get; } = 0;
            public bool isCompleted => CurrCount == NumberOfExecutions;

            public TestLongCommand(ServerThread serverThread, int numberOfExecutions)
            {
                ServerThread = serverThread;
                NumberOfExecutions = numberOfExecutions;
            }

            public void Execute() =>  CurrCount++;
            

        }

        [Fact]
        public void Server_CompletesLongOperations()
        {
            var server = new ServerThread();
            var longCommand = new TestLongCommand(server, 5);

            server.Start();
            server.AddCommand(longCommand);

            Thread.Sleep(10);

            Assert.Equal(5, longCommand.CurrCount);
            Assert.True(longCommand.isCompleted);

        }
        [Fact]
        public void Server_CompletesDifferentLongOperations()
        {
            var server = new ServerThread();

            var longCommand1 = new TestLongCommand(server, 5);
            var longCommand2 = new TestLongCommand(server, 9);

            server.Start();
            server.AddCommand(longCommand1);
            server.AddCommand(longCommand2);

            Thread.Sleep(10);

            Assert.Equal(5, longCommand1.CurrCount);
            Assert.True(longCommand1.isCompleted);

            Assert.Equal(9, longCommand2.CurrCount);
            Assert.True(longCommand2.isCompleted);

        }
        [Fact]
        public void Server_DontWorkWithWrongServer()
        {
            var output = new StringWriter();
            Console.SetOut(output);

            var server1 = new ServerThread();
            var server2 = new ServerThread();

            var longCommand = new TestLongCommand(server2, 5);

            server1.Start();
            server1.AddCommand(longCommand);

            Assert.Contains("Команда не может быть вызвана для текущего потока", output.ToString());    

        }

        [Fact]
        public void Server_CompletesCommandsWithZeroExecutions()
        {
            var server = new ServerThread();
            var longCommand = new TestLongCommand(server, 0);
            Assert.True(longCommand.isCompleted);
            server.Start();
            server.AddCommand(longCommand);
              

            Thread.Sleep(10);

            Assert.Equal(0, longCommand.CurrCount);
        }

        [Fact]
        public void Server_CompleteLongCommandsWithSoftStop()
        {
            var server = new ServerThread();

            var longCommand = new TestLongCommand(server, 5);

            server.Start();
            server.AddCommand(longCommand);
            server.AddCommand(new SoftStopCommand(server));

            Thread.Sleep(100);

            Assert.Equal(5, longCommand.CurrCount);
        }


    }
}