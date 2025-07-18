using Asp.Versioning;
using BaseApplication.Models;
using BlockChainIdentity.Application.Token.Queries.GetWalletTokens;
using BlockChainIdentity.Application.Wallet.Queries.GetWallet;
using BlockChainIdentity.Application.Wallet.Queries.GetWallets;
using BlockChainIdentity.Application.WalletRole.Queries.GetWalletRoles;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainIdentity.Grpc.Controllers.V1.Controllers
{
    [ApiVersion(1.0)]
    [Route("[controller]")]
    [ApiController]
    public class WalletsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType<PaginatedList<Application.Wallet.Queries.GetWallets.WalletDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<Application.Wallet.Queries.GetWallets.WalletDto>>> Get([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? orderBy, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new GetWalletsQuery { PageNumber = pageNumber, PageSize = pageSize, OrderBy = orderBy }, cancellationToken);
        }

        [HttpGet("{address}")]
        [ProducesResponseType<Application.Client.Queries.GetClient.ClientDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Application.Wallet.Queries.GetWallet.WalletDto>> Get(string address, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new GetWalletQuery(address), cancellationToken);
        }

        [HttpGet("{address}/Roles")]
        [ProducesResponseType<PaginatedList<WalletRoleDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<WalletRoleDto>>> GetWalletRoles(string address, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? orderBy, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new GetWalletRolesQuery(address) { PageNumber = pageNumber, PageSize = pageSize, OrderBy = orderBy }, cancellationToken);
        }

        [HttpGet("{address}/Tokens")]
        [ProducesResponseType<PaginatedList<WalletTokenDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<WalletTokenDto>>> GetWalletTokens(string address, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? orderBy, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new GetWalletTokensQuery(address) { PageNumber = pageNumber, PageSize = pageSize, OrderBy = orderBy }, cancellationToken);
        }
    }
}
