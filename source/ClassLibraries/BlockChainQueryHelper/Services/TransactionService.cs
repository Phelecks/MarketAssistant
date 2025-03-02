﻿using BlockChainQueryHelper.Interfaces;
using Nethereum.Web3;

namespace BlockChainQueryHelper.Services;

public class TransactionService : ITransactionService
{
    public async Task<Nethereum.RPC.Eth.DTOs.Transaction?> GetTransactionByHashAsync(Web3 web3, string hash, CancellationToken cancellationToken)
    {
        try
        {
            return await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(hash);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
