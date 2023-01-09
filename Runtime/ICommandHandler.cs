namespace Mediator
{
    public interface ICommandHandler<T, R> : IMediatorHandler where T : ICommand<R>
    {
        R Handle(T data);
    }
}