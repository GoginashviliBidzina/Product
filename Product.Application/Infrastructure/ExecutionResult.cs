using System.Text.Json.Serialization;

namespace Product.Application.Infrastructure
{
    public class ExecutionResult
    {
        public bool Success { get; set; }

        public DomainOperationResult Data { get; set; }

        [JsonIgnore]
        public ErrorCode ErrorCode { get; set; }
    }
}