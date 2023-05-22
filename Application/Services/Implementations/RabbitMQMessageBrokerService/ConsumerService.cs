using System.Text;
using Application.Services.Interfaces.IRabbitMQMessageBroker;
using Application.Services.MessageBrokerConfig;
using Domain.Entities;
using Identity.Data.Dtos.Request.MessageBroker;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MailKit.Net.Smtp;
using Application.DTOs;
using Application.Services.Interfaces;

namespace Application.Services.Implementations.RabbitMQMessageBrokerService
{
    public class ConsumerService : IConsumerService
    {
        private readonly IEmailService _emailService;
        private readonly EmailSender _sender;
        private readonly RabbitMQMessageBroker _rabbitMQMessageBroker;
        private readonly IConnection _connection;
        private readonly ILogger _logger;

        public ConsumerService(
            IEmailService emailService,
            IRabbitMQConfig rabbitMQConfig,
            ILogger<ConsumerService> logger,
            IOptions<RabbitMQMessageBroker> rabbitMQMessageBroker,
            IOptions<EmailSender> sender)
        {
            _emailService = emailService;
            _rabbitMQMessageBroker = rabbitMQMessageBroker.Value;
            _connection = rabbitMQConfig.CreateChannel(true);
            _logger = logger;
            _sender = sender.Value;
        }

        public void RecieveMessage(string queue)
        {
            var channel = _connection.CreateModel();
            channel = ConfigureChannel(channel, queue);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"working: {message}");
                HandleMessage(message);
                channel.BasicAck(eventArgs.DeliveryTag, false);
            };
            channel.BasicConsume(queue, false, consumer);
        }

        private Task HandleMessage(string message)
        {
            JObject json = JObject.Parse(message);
            string type = json.Value<string>("Type").ToLower();
            switch (type)
            {
                case "email":
                    var verificationPayload = JsonConvert.DeserializeObject<Notification<EmailRequest>>(message);
                    _emailService.SendEmailUsingMailKit(verificationPayload.Data);
                    break;
                default:
                    break;
            }
            //throw new NotImplementedException();
            return null;
        }

        private IModel ConfigureChannel(IModel channel, string queue)
        {
            string routingKey = _rabbitMQMessageBroker.QueueIdentityRoutingKey;
            string exchange = _rabbitMQMessageBroker.QueueIdentityExchange;
            channel.ExchangeDeclare(exchange, ExchangeType.Topic);
            channel.QueueDeclare(queue, false, false, false, null);
            channel.QueueBind(queue, exchange, routingKey, null);
            channel.BasicQos(0, 1, false);
            return channel;
        }

        public void Dispose()
        {
            if (_connection.IsOpen)
                _connection.Close();
        }        
    }
}