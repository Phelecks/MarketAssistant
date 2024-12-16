using Asp.Versioning;
using BaseApplication.Models;
using BlockChainIdentity.Application.Client.Commands.CreateClient;
using BlockChainIdentity.Application.Client.Queries.GetClient;
using BlockChainIdentity.Application.Client.Queries.GetClientResources;
using BlockChainIdentity.Application.Client.Queries.GetClients;
using BlockChainIdentity.Grpc.Controllers.V1.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainIdentity.Grpc.Controllers.V1.Controllers
{
    [ApiVersion(1.0)]
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType<PaginatedList<Application.Client.Queries.GetClients.ClientDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<Application.Client.Queries.GetClients.ClientDto>>> Get([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? orderBy, CancellationToken cancellationToken)
        {
            return await Sender.Send(new GetClientsQuery { pageNumber = pageNumber, pageSize = pageSize, orderBy = orderBy }, cancellationToken);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType<Application.Client.Queries.GetClient.ClientDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Application.Client.Queries.GetClient.ClientDto>> Get(long id, CancellationToken cancellationToken)
        {
            return await Sender.Send(new GetClientQuery(id), cancellationToken);
        }

        [HttpGet("{id:long}/Resources")]
        [ProducesResponseType<PaginatedList<Application.Client.Queries.GetClientResources.ResourceDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<Application.Client.Queries.GetClientResources.ResourceDto>>> GetClientResources(long id, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? orderBy, CancellationToken cancellationToken)
        {
            return await Sender.Send(new GetClientResourcesQuery(id) { pageNumber = pageNumber, pageSize = pageSize, orderBy = orderBy }, cancellationToken);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post(CreateClientRequest request, CancellationToken cancellationToken)
        {
            var result = await Sender.Send(new CreateClientCommand(request.ClientId, request.ClientSecret, request.Uri, request.Enabled,
                request.Statement, request.Version, request.ResourceIds, request.TokenLifeTimeInSeconds), cancellationToken);

            return Created(LinkGenerator.GetPathByAction(HttpContext, values: new { result }), result);
        }
    }
}
