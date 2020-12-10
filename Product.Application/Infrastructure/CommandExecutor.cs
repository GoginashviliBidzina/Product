using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentValidation.Attributes;
using Product.Infrastructure.DataBase;
using Product.Messaging.Infrastructure;

namespace Product.Application.Infrastructure
{
    public class CommandExecutor
    {
        private DatabaseContext _db;
        private UnitOfWork _unitOfWork;
        private MessageSender _messageSender;
        private readonly IServiceProvider _serviceProvider;

        public CommandExecutor(DatabaseContext db,
                               UnitOfWork unitOfWork,
                               MessageSender messageSender,
                               IServiceProvider serviceProvider)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _messageSender = messageSender;
            _serviceProvider = serviceProvider;
        }

        public async Task<CommandExecutionResult> ExecuteAsync(Command command)
        {
            try
            {
                var validationResult = Validate(command);

                if (!validationResult.IsValid)
                {
                    await _messageSender.SendMessageAsync(validationResult);

                    return new CommandExecutionResult
                    {
                        Success = false,
                        ErrorCode = ErrorCode.ValidationFailed,
                        Errors = validationResult.Errors.Select(error => error.ErrorMessage)
                    };
                }

                command.Resolve(_db, _unitOfWork, _messageSender, _serviceProvider);

                return await command.ExecuteAsync();
            }
            catch (Exception)
            {
                return new CommandExecutionResult
                {
                    Success = false,
                    ErrorCode = ErrorCode.Exception
                };
            }
        }

        public static ValidationResult Validate(Command execution)
        {
            var validatorAttribute = execution.GetType().GetCustomAttribute<ValidatorAttribute>(true);
            if (validatorAttribute != null)
            {
                var instance = (dynamic)Activator.CreateInstance(validatorAttribute.ValidatorType);
                var modelState = instance.Validate((dynamic)execution);
                return modelState;
            }

            return new ValidationResult();
        }
    }
}
