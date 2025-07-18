using ServerThreadSystem;
public class Program
{
    public class TestCommand : ICommand
    {
        public int id;
        public int counter = 0;
        public TestCommand(int id)
        {
            this.id = id;
        }
        public void Execute()
        {
            Console.WriteLine($"Поток {id} вызов {++counter}");
        }
    }
    public class TestCommandWithThreadName : TestCommand, IServerThreadLongCommand
    {
        public ServerThread ServerThread { get; private set; }
        public TestCommandWithThreadName(int id, ServerThread server) : base(id) => ServerThread = server;
        public bool isCompleted  => counter == 3;

    }
    public static void Main()
    {
        var server = new ServerThread();

        server.Start();

        for (int i = 1; i <= 5; i++)
        {
            server.AddCommand(new TestCommandWithThreadName(i, server));
        }
        Thread.Sleep(1000);
        server.AddCommand(new HardStopCommand(server)); 
    }
}
