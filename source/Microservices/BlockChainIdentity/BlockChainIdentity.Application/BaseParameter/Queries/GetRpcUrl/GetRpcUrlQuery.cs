using AutoMapper;
using BaseApplication.Exceptions;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.BaseParameter.Queries.GetRpcUrl;

//[Authorize(roles = "Administrators")]
public record GetRpcUrlQuery([property: Required] int chainId) : IRequest<string>;

public class Handler(IApplicationDbContext context) : IRequestHandler<GetRpcUrlQuery, string>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<string> Handle(GetRpcUrlQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.BaseParameter baseParameter = request.chainId switch
        {
            137 => await _context.baseParameters.SingleAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.BlockChainPaymentPolygonMainNetRpcUrl, cancellationToken),
            8001 => await _context.baseParameters.SingleAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.BlockChainPaymentPolygonTestNetRpcUrl, cancellationToken),
            1 => await _context.baseParameters.SingleAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.BlockChainPaymentEthereumMainNetRpcUrl, cancellationToken),
            2 => await _context.baseParameters.SingleAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.BlockChainPaymentEthereumTestNetRpcUrl, cancellationToken),
            _ => throw new NotFoundException($"Cannot find ChainId of chain {request.chainId}"),
        };
        return baseParameter.value;
    }
}
