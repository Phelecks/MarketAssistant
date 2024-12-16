// using BlockChainIdentity.Application.Wallet.Commands.AuthenticateWallet;
// using BlockChainIdentity.Application.Wallet.Queries.GetSiweMessage;
// using BlockChainIdentity.Grpc.Connect.Proto.V1;
// using Grpc.Core;
// using MediatR;

// namespace BlockChainIdentity.Grpc.Services
// {
//     public class ConnectServiceV1 : Connect.Proto.V1.Connect.ConnectBase
//     {
//         private readonly ISender _sender;
//         public ConnectServiceV1(ISender sender)
//         {
//             _sender = sender;
//         }

//         public override async Task<GetSiweMessageResponse> GetSiweMessage(GetSiweMessageRequest request, ServerCallContext context)
//         {
//             var result = await _sender.Send(new GetSiweMessageQuery(request.Address, request.ClientKey, request.ChainId), context.CancellationToken);
//             var response = new GetSiweMessageResponse
//             {
//                 SiweMessage = result.SiweMessage,
//             };

//             context.Status = new Status(StatusCode.OK, "Success");
//             return response;
//         }

//         public override async Task<GetTokenResponse> GetToken(GetTokenRequest request, ServerCallContext context)
//         {
//             var result = await _sender.Send(new AuthenticateWalletCommand(request.SiweEncodedMessage, request.Signature, request.ChainId, request.ClientKey), context.CancellationToken);
//             var response = new GetTokenResponse
//             {
//                 Address = result.Address,
//                 Token = result.Token
//             };

//             context.Status = new Status(StatusCode.OK, "Success");
//             return response;
//         }
//     }
// }