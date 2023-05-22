using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces.IRabbitMQMessageBroker
{
    public interface IProducerService : IAutoDependencyService
    {
        void SendMessage<T>(T message, string queue);
    }
}
