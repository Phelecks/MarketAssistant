using Asp.Versioning;
using BaseApplication.Models;
using BlockChainIdentity.Application.Role.Commands.CreateRole;
using BlockChainIdentity.Application.WalletRole.Commands.AddWalletToRole;
using BlockChainIdentity.Application.WalletRole.Commands.RemoveWalletFromRole;
using BlockChainIdentity.Grpc.Controllers.V1.Models;
using BlockChainIdentity.Role.Client.GetRoles.GetClients;
using BlockChainIdentity.Role.Wallet.GetRole.GetWallet;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainIdentity.Grpc.Controllers.V1.Controllers
{
    [ApiVersion(1.0)]
    [Route("[controller]")]
    [ApiController]
    public class RolesController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType<PaginatedList<Application.Role.Queries.GetRoles.RoleDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<Application.Role.Queries.GetRoles.RoleDto>>> Get([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? orderBy, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new GetRolesQuery { PageNumber = pageNumber, PageSize = pageSize, OrderBy = orderBy }, cancellationToken);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType<Application.Role.Queries.GetRole.RoleDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Application.Role.Queries.GetRole.RoleDto>> Get(long id, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new GetRoleQuery(id), cancellationToken);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post(CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var result = await Dispatcher.SendAsync(new CreateRoleCommand(request.Title), cancellationToken);

            return Created(LinkGenerator.GetPathByAction(HttpContext, values: new { result }), result);
        }

        [HttpPost("{id:long}/AddToRole")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddToRole(long id, [FromBody] string Address, CancellationToken cancellationToken)
        {
            var result =  await Dispatcher.SendAsync(new AddWalletToRoleCommand(Address, id), cancellationToken);

            return Created(LinkGenerator.GetPathByAction(HttpContext, values: new { result }), result);
        }

        [HttpPost("{id:long}/RemoveFromRole")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoveFromRole(long id, [FromBody] string Address, CancellationToken cancellationToken)
        {
            var result = await Dispatcher.SendAsync(new RemoveWalletFromRoleCommand(Address, id), cancellationToken);

            return Created(LinkGenerator.GetPathByAction(HttpContext, values: new { result }), result);
        }
    }
}
