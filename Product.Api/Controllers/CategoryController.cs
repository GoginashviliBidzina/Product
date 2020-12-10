using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Infrastructure;
using Product.Application.Commands;
using Product.Application.Infrastructure;

namespace Product.Api.Controllers
{
    [Route("v1/Category")]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        public CommandExecutor _commandExecutor;

        public CategoryController(CommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] PlaceCategoryCommand command)
        {
            var result = await _commandExecutor.ExecuteAsync(command);

            return CommandResultToHttpResponse(result, EntityStatusCode.Created);
        }
    }
}
