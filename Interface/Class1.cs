public interface ICommand
{
    void Execute();
}
public interface IExceptionHandler
{
    void HandleException(ICommand command, Exception ex);

}
public interface IScheduler
{
    bool HasCommand();
    ICommand Select();
    void Add(ICommand cmd);
}

public interface ILongCommand : ICommand
{
    public bool isCompleted { get; }
}
