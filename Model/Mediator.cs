using System;
using System.Collections.Generic;
using System.Linq;

namespace EsriCarRentalApp
{
    public class Mediator : IMediator
    {
        private readonly Dictionary<Type, List<object>> _messageHandlers = new Dictionary<Type, List<object>>();

        public void SendMessage<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);

            if (_messageHandlers.ContainsKey(messageType))
            {
                foreach (var handler in _messageHandlers[messageType].ToList())
                {
                    ((Action<TMessage>)handler)(message);
                }
            }
        }

        public void Register<TMessage>(Action<TMessage> action)
        {
            var messageType = typeof(TMessage);

            if (!_messageHandlers.ContainsKey(messageType))
            {
                _messageHandlers[messageType] = new List<object>();
            }

            _messageHandlers[messageType].Add(action);
        }

        public void Unregister<TMessage>(Action<TMessage> action)
        {
            var messageType = typeof(TMessage);

            if (_messageHandlers.ContainsKey(messageType))
            {
                _messageHandlers[messageType].Remove(action);
            }
        }
    }

}
