using System.Collections.Concurrent;
using Scheduler;

namespace ServerThreadSystem
{
    public interface IServerThreadCommand : ICommand
    {
        public ServerThread ServerThread { get; }

    }
    public interface IServerThreadLongCommand : IServerThreadCommand, ILongCommand;
    public class HardStopCommand : IServerThreadCommand
    {
        public ServerThread ServerThread {  get; private set; }

        public HardStopCommand(ServerThread serverThread) => ServerThread = serverThread;

        public void Execute()
        {
            if (Thread.CurrentThread != ServerThread.Thread) ServerThread.ExceptionHandler
                     .HandleException(this, new Exception("HardStop не может быть вызыван для текущего потока"));

            ServerThread.IsWorking = false;
        }
    }
    public class SoftStopCommand : IServerThreadCommand
    {
        public ServerThread ServerThread { get; private set; }

        public SoftStopCommand(ServerThread serverThread) => ServerThread = serverThread;

        public void Execute()
        {
            if (Thread.CurrentThread != ServerThread.Thread) ServerThread.ExceptionHandler
                     .HandleException(this, new Exception("SoftStop не может быть вызыван для текущего потока"));

            ServerThread.SoftStop = true;
        }

    }

    public class DefaultExceptionHandler : IExceptionHandler
    {
        public void HandleException(ICommand command, Exception ex) =>
            Console.WriteLine($"Команда {command.GetType()} вызвала исключение с сообщением {ex.Message}");
    }
    public class ServerThread
    {
        public Thread Thread { get; }

        public BlockingCollection<IServerThreadCommand> Commands { get; private set; } = new BlockingCollection<IServerThreadCommand>();
        public IScheduler Scheduler { get; private set; } = new RoundRobinScheduler();
        public bool IsWorking { get; internal set; } = false;
        public bool SoftStop { get; internal set; } = false;
        public IExceptionHandler ExceptionHandler { get; private set; } = new DefaultExceptionHandler();
        public ServerThread() => Thread = new Thread(_ => Work());
        public ServerThread(IExceptionHandler exceptionHandler)
        {
            ExceptionHandler = exceptionHandler;
            Thread = new Thread(_ => Work());
        }
        private void Work()
        {
            while (IsWorking && !(Commands.Count == 0 && SoftStop && !Scheduler.HasCommand()))
            {

                if (Commands.TryTake(out var currCommand))
                {
                    try
                    {
                        if (currCommand is IServerThreadLongCommand longCurrCommand) Scheduler.Add(currCommand);
                        else currCommand.Execute();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(currCommand, ex);
                    }
                }

                if (Scheduler.HasCommand())
                {
                    var schedulerCurrCommand = Scheduler.Select();
                    if (schedulerCurrCommand == null) continue;
                    try
                    {
                        schedulerCurrCommand.Execute();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(schedulerCurrCommand, ex);
                    }
                }

            }
            IsWorking = false;
        }
        public void Start()
        {
            if (IsWorking) return;

            IsWorking = true;
            Thread.Start();
        }
        public void AddCommand(IServerThreadCommand command)
        {
            if (!IsWorking) ExceptionHandler.HandleException(command, new Exception("Сервер не запущен"));
            if (command.ServerThread != this) ExceptionHandler.HandleException(command, new Exception("Команда не может быть вызвана для текущего потока"));

            Commands.Add(command);
        }
    }
}