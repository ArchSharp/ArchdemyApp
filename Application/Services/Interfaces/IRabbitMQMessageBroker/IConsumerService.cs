using Application.Helpers;
using System;

namespace Application.Services.Interfaces.IRabbitMQMessageBroker
{
    public interface IConsumerService : IAutoDependencyService, IDisposable
    {
        public void RecieveMessage(string queue);
    }
}