using System.Collections.Generic;
//采用命令模式处理一些命令
public interface ICommand
{
    void Execute();
}
//命令队列
public class CommandQueue
{
    private Queue<ICommand> commandQueue = new Queue<ICommand>();
    public void AddCommand(ICommand command)
    {
        commandQueue.Enqueue(command);
    }
    public void ExecuteCommands()
    {
        while (commandQueue.Count > 0)
        {
            ICommand command = commandQueue.Dequeue();
            command.Execute();
        }
    }
}