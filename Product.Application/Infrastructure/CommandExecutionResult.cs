using System.Collections.Generic;

namespace Product.Application.Infrastructure
{
    public class CommandExecutionResult : ExecutionResult
    {
        public IEnumerable<string> Errors { get; set; }
    }
}