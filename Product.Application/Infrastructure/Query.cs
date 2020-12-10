using System.Threading.Tasks;

namespace Product.Application.Infrastructure
{
    public abstract class Query<TQueryResult> : ApplicationBase
        where TQueryResult : class
    {
        public abstract Task<QueryExecutionResult<TQueryResult>> ExecuteAsync();

        protected async Task<QueryExecutionResult<TQueryResult>> FailAsync(ErrorCode errorCode)
        {
            var result = new QueryExecutionResult<TQueryResult>
            {
                Success = false,
            };

            return await Task.FromResult(result);
        }

        protected async Task<QueryExecutionResult<TQueryResult>> OkAsync(TQueryResult data)
        {
            var result = new QueryExecutionResult<TQueryResult>
            {
                Data = data,
                Success = true
            };

            return await Task.FromResult(result);
        }
    }
}
