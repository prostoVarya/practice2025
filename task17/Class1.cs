using System;
using System.Collections.Concurrent;
using System.Threading;
using Command;
namespace task17
{
    public class ServerThread : IDisposable
    {
        private Thread _workerThread;
        private volatile bool _isRunning;
        private volatile bool _softStopRequested;
        private readonly BlockingCollection<ICommand> _commandQueue = new BlockingCollection<ICommand>();
        private IExceptionHandler _exceptionHandler;

        public void Start()
        {
            if (_workerThread != null && _workerThread.IsAlive)
                return;

            _isRunning = true;
            _softStopRequested = false;
            _workerThread = new Thread(ProcessCommands)
            {
                IsBackground = true,
                Name = "ServerThread Worker"
            };
            _workerThread.Start();
        }

        public void AddCommand(ICommand command)
        {
            if (!_isRunning)
                throw new InvalidOperationException("ServerThread не запущен");

            _commandQueue.Add(command);
        }

        public void SetExceptionHandler(IExceptionHandler handler)
        {
            _exceptionHandler = handler;
        }

        private void ProcessCommands()
        {
            try
            {
                while (_isRunning)
                {
                    if (_commandQueue.TryTake(out ICommand command, Timeout.Infinite))
                    {
                        ExecuteCommand(command);
                    }

                    if (_softStopRequested && _commandQueue.Count == 0)
                    {
                        _isRunning = false;
                    }
                }
            }
            finally
            {
                _commandQueue.CompleteAdding();
            }
        }

        private void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                _exceptionHandler?.HandleException(command, ex);
            }
        }

        public void RequestHardStop()
        {
            if (Thread.CurrentThread != _workerThread)
                throw new InvalidOperationException("HardStop можно вызвать только из ServerThread");

            _isRunning = false;
        }

        public void RequestSoftStop()
        {
            if (Thread.CurrentThread != _workerThread)
                throw new InvalidOperationException("SoftStop можно вызвать только из ServerThread");

            _softStopRequested = true;
        }

        public void Dispose()
        {
            _isRunning = false;
            _commandQueue.Dispose();
            _workerThread?.Join();
        }
    }

    public interface IExceptionHandler
    {
        void HandleException(ICommand command, Exception exception);
    }

    public class HardStopCommand : ICommand
    {
        private readonly ServerThread _targetThread;

        public HardStopCommand(ServerThread targetThread)
        {
            _targetThread = targetThread ?? throw new ArgumentNullException(nameof(targetThread));
        }

        public void Execute()
        {
            _targetThread.RequestHardStop();
        }
    }

    public class SoftStopCommand : ICommand
    {
        private readonly ServerThread _targetThread;

        public SoftStopCommand(ServerThread targetThread)
        {
            _targetThread = targetThread ?? throw new ArgumentNullException(nameof(targetThread));
        }

        public void Execute()
        {
            _targetThread.RequestSoftStop();
        }
    }
}