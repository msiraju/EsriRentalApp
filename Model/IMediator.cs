using System;

namespace EsriCarRentalApp
{
    public interface IMediator
    {
        void SendMessage<TMessage>(TMessage message);
        void Register<TMessage>(Action<TMessage> action);
        void Unregister<TMessage>(Action<TMessage> action);
    }

}
