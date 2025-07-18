using MediatR.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Informing.Grpc.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private IRequestDispatcher _dispatcher = null!;
    private LinkGenerator _linkGenerator = null!;

    protected IRequestDispatcher dispatcher => _dispatcher ??= HttpContext.RequestServices.GetRequiredService<IRequestDispatcher>();
    protected LinkGenerator LinkGenerator => _linkGenerator ??= HttpContext.RequestServices.GetRequiredService<LinkGenerator>();

    //private readonly IRequestDispatcher dispatcher;

    //public ApiControllerBase(IRequestDispatcher dispatcher)
    //{
    //    Sender = dispatcher;
    //}
}