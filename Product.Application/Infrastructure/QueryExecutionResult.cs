namespace Product.Application.Infrastructure
{
    public class QueryExecutionResult<T>
    {
        public QueryExecutionResult()
        {
        }

        public bool Success { get; set; }

        public T Data { get; set; }

        public ErrorCode ErrorCode { get; set; }
    }
}