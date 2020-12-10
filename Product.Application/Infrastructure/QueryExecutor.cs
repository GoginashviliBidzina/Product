using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentValidation.Attributes;
using Product.Infrastructure.DataBase;
using Product.Messaging.Infrastructure;

namespace Product.Application.Infrastructure
{
    public class QueryExecutor
    {
        private DatabaseContext _db;
        private UnitOfWork _unitOfWork;
        private MessageSender _messageSender;
        private readonly IServiceProvider _serviceProvider;

        public QueryExecutor(DatabaseContext db,
                             UnitOfWork unitOfWork,
                             MessageSender messageSender,
                             IServiceProvider serviceProvider)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _messageSender = messageSender;
            _serviceProvider = serviceProvider;
        }

        public async Task<QueryExecutionResult<TResult>> ExecuteAsync<TQuery, TResult>(TQuery query)
            where TQuery : Query<TResult>
            where TResult : class
        {
            try
            {
                var validationResult = Validate(query);

                if (!validationResult.IsValid)
                {
                    return new QueryExecutionResult<TResult>
                    {
                        Success = false,
                        ErrorCode = ErrorCode.ValidationFailed
                    };
                }

                query.Resolve(_db, _unitOfWork, _messageSender, _serviceProvider);

                return await query.ExecuteAsync();
            }
            catch (Exception)
            {
                return new QueryExecutionResult<TResult>
                {
                    Success = false,
                    ErrorCode = ErrorCode.Exception
                };
            }
        }

        public static ValidationResult Validate<TResult>(Query<TResult> execution) where TResult : class
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
