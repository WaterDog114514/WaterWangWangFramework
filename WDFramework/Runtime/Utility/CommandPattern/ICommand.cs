using System.Collections.Generic;
//��������ģʽ����һЩ����
public interface ICommand
{
    void Execute();
}
//�������
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