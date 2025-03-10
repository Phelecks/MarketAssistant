﻿using BlockProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.WalletAddress.Queries.GetAllAddresses;

public record GetAllAddressesQuery : IRequest<List<string>>;

public class Handler(IApplicationDbContext context) : IRequestHandler<GetAllAddressesQuery, List<string>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<List<string>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
    {
        return await _context.WalletAddresses.Select(s => s.Address).ToListAsync(cancellationToken);
    }
}
