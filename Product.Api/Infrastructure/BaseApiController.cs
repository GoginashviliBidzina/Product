using Microsoft.AspNetCore.Mvc;
using Product.Application.Infrastructure;

namespace Product.Api.Infrastructure
{
    public class BaseApiController : ControllerBase
    {
        protected IActionResult CommandResultToHttpResponse(CommandExecutionResult result, EntityStatusCode entityStatusCode)
        {
            if (result.Success)
            {
                return entityStatusCode switch
                {
                    EntityStatusCode.Created => Created("", result.Data),
                    EntityStatusCode.Updated => Ok(result.Data),
                    EntityStatusCode.Deleted => NoContent(),
                    _ => BadRequest(),
                };
            }

            if (result.ErrorCode == ErrorCode.NotFound)
                return NotFound();

            if (result.ErrorCode == ErrorCode.ValidationFailed)
                return BadRequest(result.Errors);

            return BadRequest(result.ErrorCode);
        }

        protected IActionResult QueryResultToHttpResponse<T>(QueryExecutionResult<T> result)
        {
            if (result.Success)
                return Ok(result.Data);

            if (result.ErrorCode == ErrorCode.NotFound)
                return NotFound();

            return BadRequest();
        }
    }

    public enum EntityStatusCode
    {
        Created = 1,
        Updated = 2,
        Deleted = 3
    }
}
