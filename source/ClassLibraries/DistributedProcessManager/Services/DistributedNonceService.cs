using CacheManager.Interfaces;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.NonceServices;
using System.Numerics;

namespace DistributedProcessManager.Services;

public class DistributedNonceService : INonceService
{
    private readonly string _accountAddress;
    private readonly IDistributedLockService _distributedLockService;
    public IClient Client { get; set; }
    public BigInteger CurrentNonce { get; set; } = -1;
    public bool UseLatestTransactionsOnly { get; set; }

    public DistributedNonceService(string accountAddress, IClient client, IDistributedLockService distributedLockService, bool useLatestTransactionsOnly = false)
    {
        Client = client;
        _accountAddress = accountAddress;
        _distributedLockService = distributedLockService;
        UseLatestTransactionsOnly = useLatestTransactionsOnly;
    }

    public async Task<HexBigInteger> GetNextNonceAsync()
    {
        HexBigInteger nextNonce = new HexBigInteger(BigInteger.Zero);
        if (Client == null)
        {
            throw new NullReferenceException("Client not configured");
        }

        EthGetTransactionCount ethGetTransactionCount = new EthGetTransactionCount(Client);

        await _distributedLockService.RunWithLockAsync(func: async () =>
        {
            try
            {
                BlockParameter block = BlockParameter.CreatePending();
                if (UseLatestTransactionsOnly)
                {
                    block = BlockParameter.CreateLatest();
                }

                HexBigInteger hexBigInteger = 
                    await ethGetTransactionCount.SendRequestAsync(_accountAddress, block).
                        ConfigureAwait(continueOnCapturedContext: false);
                if (hexBigInteger.Value <= CurrentNonce)
                {
                    CurrentNonce += (BigInteger)1;
                    hexBigInteger = new HexBigInteger(CurrentNonce);
                }
                else
                {
                    CurrentNonce = hexBigInteger.Value;
                }

                nextNonce = hexBigInteger;
            }
            catch(Exception exception)
            {
                throw new Exception($"An error occurred during get next nonce for account: {_accountAddress}, {exception.Message}");
            }
        }, $"Nonce_{_accountAddress}");

        //if(nextNonce.Value.IsZero) 
        //    throw new Exception($"An error occurred during get next nonce for account: {_accountAddress}.");

        return nextNonce;
    }

    public async Task ResetNonceAsync()
    {
        await _distributedLockService.RunWithLockAsync(func: async () =>
        {
            try
            {
                CurrentNonce = -1;
                await Task.CompletedTask;
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred during reset nonce for account: {_accountAddress}.");
            }
        }, $"Nonce_{_accountAddress}");
    }
}