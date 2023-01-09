using System.Linq;

namespace Mediator
{
    public static class Mediator
    {
        private static MediatorHandlerHolder commandHandlers;
        private static MediatorHandlerHolder eventHandlers;

        static Mediator()
        {
            commandHandlers = new MediatorHandlerHolder();
            eventHandlers = new MediatorHandlerHolder();
        }

        public static void Subscribe(object handler)
        {
            var type = handler.GetType();
            var allInterfaces = type.GetInterfaces();
            foreach (var _interface in allInterfaces)
            {
                if (_interface.Name == typeof(IEventHandler<>).Name ||
                    _interface.Name == typeof(ICommandHandler<,>).Name)
                {
                    commandHandlers.Add(handler as IMediatorHandler, _interface);
                }
            }
        }

        public static void Unsubscribe(object handler)
        {
            var type = handler.GetType();
            var allInterfaces = type.GetInterfaces();
            foreach (var _interface in allInterfaces)
            {
                if (_interface.Name == typeof(IEventHandler<>).Name ||
                    _interface.Name == typeof(ICommandHandler<,>).Name)
                {
                    commandHandlers.Remove(handler as IMediatorHandler, _interface);
                }
            }
        }

        public static void Publish<T>(T eventToPublish) where T : IEvent
        {
            var relatedHandlers = eventHandlers.GetHandlers(typeof(IEventHandler<T>));
            foreach (var handler in relatedHandlers)
            {
                var convertedHandler = handler as IEventHandler<T>;
                convertedHandler.Handle(eventToPublish);
            }
        }

        public static R Send<T, R>(T commandToSend) where T : ICommand<R>
        {
            var relatedHandlers = commandHandlers.GetHandlers(typeof(ICommandHandler<T, R>));
            var handler = relatedHandlers.FirstOrDefault();
            if (handler != null)
            {
                var convertedHandler = handler as ICommandHandler<T, R>;
                return convertedHandler.Handle(commandToSend);
            }
            return default(R);
        }

        public static R Send<T,R>() where T : ICommand<R>, new()
        {
            return Send<T, R>(new T());
        }
    }
}
