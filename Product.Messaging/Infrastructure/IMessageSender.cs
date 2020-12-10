using FluentValidation.Results;
using System.Threading.Tasks;
using Template.Shared;

namespace Product.Messaging.Infrastructure
{
    public interface IMessageSender
    {
        Task SendMessageAsync(ValidationResult validationResult);

        Task SendMessageAsync<TAggregate>(TAggregate aggregateRoot) where TAggregate : AggregateRoot;
    }
}
