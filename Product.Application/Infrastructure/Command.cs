using System.Linq;
using Template.Shared;
using System.Threading.Tasks;

namespace Product.Application.Infrastructure
{
    public abstract class Command : ApplicationBase
    {
        public abstract Task<CommandExecutionResult> ExecuteAsync();

        protected Task<CommandExecutionResult> FailAsync(ErrorCode errorCode)
        {
            var result = new CommandExecutionResult
            {
                Success = false,
                ErrorCode = errorCode
            };

            return Task.FromResult(result);
        }

        protected Task<CommandExecutionResult> FailAsync(ErrorCode errorCode, string[] errorMessages)
        {
            var result = new CommandExecutionResult
            {
                Success = false,
                ErrorCode = errorCode
            };

            if (!errorMessages.Any())
                result.Errors = errorMessages;

            return Task.FromResult(result);
        }

        protected async Task<CommandExecutionResult> OkAsync(DomainOperationResult data)
        {
            var result = new CommandExecutionResult
            {
                Data = data,
                Success = true
            };

            return await Task.FromResult(result);
        }

        public async Task SaveAsync<TAggregate>(TAggregate aggregate, IRepository<TAggregate> repository) where TAggregate : AggregateRoot
        {
            repository.Save(aggregate);

            await _unitOfWork.SaveAsync();
        }
    }
}
