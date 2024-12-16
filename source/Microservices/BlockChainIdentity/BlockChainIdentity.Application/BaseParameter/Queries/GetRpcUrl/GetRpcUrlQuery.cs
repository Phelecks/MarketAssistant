using AutoMapper;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Application.BaseParameter.Queries.GetRpcUrl;

//[Authorize(roles = "Administrators")]
public record GetRpcUrlQuery([property: Required] int chainId) : IRequest<string>;

public class Handler : IRequestHandler<GetRpcUrlQuery, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public Handler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<string> Handle(GetRpcUrlQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.BaseParameter baseParameter;
        switch (request.chainId)
        {
            case 137:
                baseParameter = await _context.baseParameters.SingleAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.BlockChainPaymentPolygonMainNetRpcUrl, cancellationToken);
                break;
            case 8001:
                baseParameter = await _context.baseParameters.SingleAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.BlockChainPaymentPolygonTestNetRpcUrl, cancellationToken);
                break;
            case 1:
                baseParameter = await _context.baseParameters.SingleAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.BlockChainPaymentEthereumMainNetRpcUrl, cancellationToken);
                break;
            case 2:
                baseParameter = await _context.baseParameters.SingleAsync(exp => exp.field == BaseDomain.Enums.BaseParameterField.BlockChainPaymentEthereumTestNetRpcUrl, cancellationToken);
                break;
            default:
                throw new Exception($"Cannot find ChainId of chain {request.chainId}");
        }
        return baseParameter.value;
    }
}
