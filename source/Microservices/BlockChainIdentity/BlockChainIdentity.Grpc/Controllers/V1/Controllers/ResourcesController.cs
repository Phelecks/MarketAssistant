using Asp.Versioning;
using BaseApplication.Models;
using BlockChainIdentity.Application.Resource.Queries.GetResource;
using BlockChainIdentity.Application.Resource.Queries.GetResources;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainIdentity.Grpc.Controllers.V1.Controllers
{
    [ApiVersion(1.0)]
    [Route("[controller]")]
    [ApiController]
    public class ResourcesController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType<PaginatedList<Application.Resource.Queries.GetResources.ResourceDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<Application.Resource.Queries.GetResources.ResourceDto>>> Get([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? orderBy, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new GetResourcesQuery { PageNumber = pageNumber, PageSize = pageSize, OrderBy = orderBy }, cancellationToken);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType<Application.Resource.Queries.GetResource.ResourceDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Application.Resource.Queries.GetResource.ResourceDto>> Get(long id, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new GetResourceQuery(id), cancellationToken);
        }
    }
}
