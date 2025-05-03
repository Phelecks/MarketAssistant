using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlockChainIdentity.Grpc.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _sender = null!;
    private LinkGenerator _linkGenerator = null!;

    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected LinkGenerator LinkGenerator => _linkGenerator ??= HttpContext.RequestServices.GetRequiredService<LinkGenerator>();

    //private readonly ISender Sender;

    //public ApiControllerBase(ISender sender)
    //{
    //    Sender = sender;
    //}
}