using EsriCarRentalApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EsriCarRentalAppTests
{
    [TestClass]
    public class MediatorTests
    {
        [TestMethod]
        public void SendMessage_NoHandlers_NoException()
        {
            var mediator = new Mediator();
            var message = new SampleMessage();

            Assert.ThrowsException<ArgumentException>(() => mediator.SendMessage(message));
        }

        [TestMethod]
        public void SendMessage_WithHandlers_HandlersCalled()
        {
            var mediator = new Mediator();
            var message = new SampleMessage();
            bool handler1Called = false;
            bool handler2Called = false;

            mediator.Register<Action<SampleMessage>>(m => handler1Called = true);
            mediator.Register<Action<SampleMessage>>(m => handler2Called = true);

            mediator.SendMessage(message);

            Assert.IsTrue(handler1Called);
            Assert.IsTrue(handler2Called);
        }

        [TestMethod]
        public void Register_HandlerAddedSuccessfully()
        {
            var mediator = new Mediator();
            var handlerCalled = false;
            Action<SampleMessage> handler = m => handlerCalled = true;

            mediator.Register(handler);
            mediator.SendMessage(new SampleMessage());

            Assert.IsTrue(handlerCalled);
        }

        [TestMethod]
        public void Unregister_HandlerRemovedSuccessfully()
        {
            var mediator = new Mediator();
            var handlerCalled = false;
            Action<SampleMessage> handler = m => handlerCalled = true;

            mediator.Register(handler);

            mediator.Unregister(handler);
            mediator.SendMessage(new SampleMessage());

            Assert.IsFalse(handlerCalled);
        }

        [TestMethod]
        public void Unregister_HandlerNotRegistered_NoException()
        {
            var mediator = new Mediator();
            Action<SampleMessage> handler = m => { };

            Assert.ThrowsException<ArgumentException>(() => mediator.Unregister(handler));
        }

        private class SampleMessage { }
    }

}
