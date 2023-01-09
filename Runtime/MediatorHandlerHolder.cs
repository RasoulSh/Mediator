using System;
using System.Collections.Generic;

namespace Mediator
{
    internal class MediatorHandlerHolder
    {
        private static Dictionary<Type, List<IMediatorHandler>> handlerDict;
        public MediatorHandlerHolder()
        {
            handlerDict = new Dictionary<Type, List<IMediatorHandler>>();
        }

        public void Add(IMediatorHandler handler, Type type)
        {
            List<IMediatorHandler> value;
            if (handlerDict.TryGetValue(type, out value))
            {
                if (value.Contains(handler))
                {
                    throw new Exception("You are subscribing more than once");
                }
                else
                {
                    value.Add(handler);
                }
            }
            else
            {
                handlerDict.Add(type, new List<IMediatorHandler>() { handler });
            }
        }

        public void Remove(IMediatorHandler handler, Type type)
        {
            List<IMediatorHandler> value;
            if (handlerDict.TryGetValue(type, out value))
            {
                value.Remove(handler);
            }
        }

        public List<IMediatorHandler> GetHandlers(Type handlerType)
        {
            List<IMediatorHandler> result;
            if (handlerDict.TryGetValue(handlerType, out result))
            {
                if (result != null)
                {
                    return result;
                }
            }
            return new List<IMediatorHandler>();
        }
    }
}