using System;
using System.Linq;
using System.Text;
using Template.Shared;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Product.Messaging.Models;
using FluentValidation.Results;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Product.Messaging.Configurations;

namespace Product.Messaging.Infrastructure
{
    public class MessageSender : IMessageSender
    {
        private QueueClient _client;
        private readonly AzureServiceBusConfig _config;

        public MessageSender(IOptions<AzureServiceBusConfig> config)
        {
            _config = config.Value;
        }

        public async Task SendMessageAsync(ValidationResult validationResult)
        {
            try
            {
                if (ConnectionExists())
                {
                    foreach (var validation in validationResult.Errors.Select(error => error.ErrorMessage))
                    {
                        var @event = new DefaultEventModel(validation, DateTime.Now);
                        var serializedEvent = JsonConvert.SerializeObject(@event);

                        var message = new Message(Encoding.UTF8.GetBytes(serializedEvent))
                        {
                            Label = @event.GetType().ToString().ToLower()
                        };

                        message.UserProperties.Add("eventType", nameof(DefaultEventModel));

                        await _client.SendAsync(message);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendMessageAsync<TAggregate>(TAggregate aggregateRoot) where TAggregate : AggregateRoot
        {
            try
            {
                if (ConnectionExists())
                {
                    var domainEvents = (aggregateRoot as IHasDomainEvents).UncommittedChanges();

                    foreach (var domainEvent in domainEvents)
                    {
                        var serializedEvent = JsonConvert.SerializeObject(domainEvent);
                        var message = new Message(Encoding.UTF8.GetBytes(serializedEvent))
                        {
                            Label = domainEvent.EventType.ToLower()
                        };

                        message.UserProperties.Add("eventType", domainEvent.EventType);

                        await _client.SendAsync(message);
                        if (domainEvents.Count() == 0)
                            break;
                    }
                }
            }
            catch (System.Exception exception)
            {
                throw;
            }
        }

        private void CreateConnection()
            => _client = new QueueClient($"{_config.ConnectionString}", _config.QueueName, ReceiveMode.PeekLock);

        private bool ConnectionExists()
        {
            if (_client == null)
                CreateConnection();

            return _client != null;
        }
    }
}
