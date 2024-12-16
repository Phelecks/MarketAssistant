using Asp.Versioning;
using Informing.Application.Information.Commands.CreateTestSignalRMessage;
using Microsoft.AspNetCore.Mvc;

namespace Informing.Grpc.Controllers.V1.Controllers
{
    [ApiVersion(1.0)]
    [Route("[controller]")]
    [ApiController]
    public class TestsController : ApiControllerBase
    {
        [HttpPost("CreateTestSignalRMessageCommand")]
        public async Task<ActionResult> CreateTestSignalRMessageCommand(CancellationToken cancellationToken)
        {
            await Sender.Send(new CreateTestSignalRMessageCommand(), cancellationToken);
            return Ok();
        }
    }
}
