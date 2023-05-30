using System.Threading;
using System.Threading.Tasks;
using Application.Services.Interfaces.IRabbitMQMessageBroker;
using Domain.Entities.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services.Implementations
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly RabbitMQMessageBroker _rabbitMQMessageBroker;
        private readonly IConsumerService _consumerService;

        public ConsumerHostedService(
            ILogger<ConsumerHostedService> logger,
            IConsumerService consumerService,
            IOptions<RabbitMQMessageBroker> rabbitMQMessageBroker)
        {
            _logger = logger;
            _consumerService = consumerService;
            _rabbitMQMessageBroker = rabbitMQMessageBroker.Value;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Background service starting...");
            _consumerService.RecieveMessage(_rabbitMQMessageBroker.QueueNotification);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _consumerService.Dispose();
            base.Dispose();
        }
    }
}