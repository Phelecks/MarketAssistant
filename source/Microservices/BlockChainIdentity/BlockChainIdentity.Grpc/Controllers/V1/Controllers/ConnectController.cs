using Asp.Versioning;
using BlockChainIdentity.Application.Wallet.Commands.AuthenticateWallet;
using BlockChainIdentity.Application.Wallet.Queries.GetSiweMessage;
using BlockChainIdentity.Grpc.Controllers.V1.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainIdentity.Grpc.Controllers.V1.Controllers
{
    [ApiVersion(1.0)]
    [Route("[controller]")]
    [ApiController]
    public class ConnectController : ApiControllerBase
    {
        /// <summary>
        /// Get Siwe message
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("siwe")]
        [ProducesResponseType<SiweMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SiweMessageDto>> GetSiweMessage([FromQuery] GetSiweRequest request, [FromHeader(Name = "ClientKey")] string clientKey, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new GetSiweMessageQuery(request.Address, clientKey, request.ChainId), cancellationToken);
        }

        /// <summary>
        /// Get token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("token")]
        [ProducesResponseType<SiweMessageDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenDto>> Token([FromBody] GetTokenRequest request, [FromHeader(Name = "ClientKey")] string clientKey, CancellationToken cancellationToken)
        {
            return await Dispatcher.SendAsync(new AuthenticateWalletCommand(request.SiweEncodedMessage, request.Signature, request.ChainId, clientKey), cancellationToken);
        }
    }
}
