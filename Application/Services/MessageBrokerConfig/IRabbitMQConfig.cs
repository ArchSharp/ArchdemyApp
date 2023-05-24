using Application.Helpers;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.MessageBrokerConfig
{
    public interface IRabbitMQConfig : IAutoDependencyService
    {
        IConnection CreateRabbitMQConnection(bool async);
    }
}
