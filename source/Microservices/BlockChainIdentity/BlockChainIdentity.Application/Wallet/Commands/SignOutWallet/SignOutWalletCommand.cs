// using BaseApplication.Security;
// using BlockChainIdentity.Application.Interfaces;
// using BlockChainIdentity.Domain.Events.Wallet;
// using MediatR.Interfaces;
// using Microsoft.AspNetCore.Http;
// using Microsoft.EntityFrameworkCore;
// using Nethereum.Siwe;
// using Nethereum.Siwe.Core;

// namespace BlockChainIdentity.Application.Wallet.Commands.SignOutWallet;

// [Authorize()]
// public record SignOutWalletCommand() : IRequest<Unit>;

// public class Handler : IRequestHandler<SignOutWalletCommand, Unit>
// {
//     private readonly IApplicationDbContext _context;
//     private readonly SiweMessageService _siweMessageService;
//     private readonly IHttpContextAccessor _httpContext;

//     private const string ContextSiweMessage = "siweMessage";

//     public Handler(IApplicationDbContext context, SiweMessageService siweMessageService, IHttpContextAccessor httpContext)
//     {
//         _context = context;
//         _siweMessageService = siweMessageService;
//         _httpContext = httpContext;
//     }

//     public async Task<Unit> HandleAsync(SignOutWalletCommand request, CancellationToken cancellationToken)
//     {
//         if (_httpContext != null && _httpContext.HttpContext != null && 
//             _httpContext.HttpContext.Items.ContainsKey(ContextSiweMessage))
//         {
//             var siweMessage = (SiweMessage)_httpContext.HttpContext.Items[ContextSiweMessage]!;
//             _siweMessageService.InvalidateSession(siweMessage);

//             var token = await _context.tokens.Include(inc => inc.wallet).SingleOrDefaultAsync(exp => exp.walletAddress.Equals(siweMessage.Address) && exp.requestId.Equals(siweMessage.RequestId), cancellationToken);
//             if(token != null)
//             {
//                 token.enabled = false;
//                 token.AddDomainNotification(new WalletSignedOutEvent(token.wallet));
//                 await _context.SaveChangesAsync(cancellationToken);
//             }
//         }
//         return Unit.Value;
//     }
// }
