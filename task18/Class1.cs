namespace Scheduler
{
    public class RoundRobinScheduler : IScheduler
    {
        public List<ICommand> Commands { get; private set; } = new List<ICommand>();
        private readonly object locker = new object();
        private int currIndex = 0;

        public bool HasCommand() 
        {
            lock (locker)
            {
                return Commands.Count != 0;
            }
        }

        public ICommand Select()
        {
            lock (locker)
            {
                if (!this.HasCommand()) throw new Exception("Список команд для выполнения пуст");

                ICommand command = Commands[currIndex];

                if (command is ILongCommand longCommand)
                {
                    if (longCommand.isCompleted)
                    {
                        Commands.Remove(command);

                        if (!this.HasCommand()) return null;

                        return Commands[currIndex];
                    }
                }

                else
                {
                    Commands.Remove(command);
                    if (!this.HasCommand()) return null;
                    return Commands[currIndex];
                }

                currIndex = (currIndex + 1) % Commands.Count;
                return command;

            }
        }
        public void Add(ICommand command)
        {
            if (command is ILongCommand longCommand && (longCommand.isCompleted || Commands.Contains(longCommand))) return;
            
             Commands.Add(command);
        }

    }
}