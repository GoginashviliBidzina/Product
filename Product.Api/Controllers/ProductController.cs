using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Product.Api.Infrastructure;
using Product.Application.Queries;
using Product.Application.Commands;
using Product.Application.Infrastructure;

namespace Product.Api.Controllers
{
    [Route("v1/product")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        public QueryExecutor _queryExecutor;
        public CommandExecutor _commandExecutor;

        public ProductController(QueryExecutor queryExecutor,
                                 CommandExecutor commandExecutor)
        {
            _queryExecutor = queryExecutor;
            _commandExecutor = commandExecutor;
        }

        [HttpGet("listing")]
        public async Task<IActionResult> Listing([FromQuery] ProductListingQuery query)
        {
            var result = await _queryExecutor.ExecuteAsync<ProductListingQuery, IEnumerable<ProductListingQueryResult>>(query);

            return QueryResultToHttpResponse(result);
        }

        [HttpGet("details")]
        public async Task<IActionResult> Details([FromQuery] ProductDetailsQuery query)
        {
            var result = await _queryExecutor.ExecuteAsync<ProductDetailsQuery, ProductDetailsQueryResult>(query);

            return QueryResultToHttpResponse(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] PlaceProductCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResultToHttpResponse(result, EntityStatusCode.Created);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteProductCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResultToHttpResponse(result, EntityStatusCode.Deleted);
        }
    }
}
