namespace CacheManager.Helpers;

public static class MutexKeys
{
    private const string FinancialSubAccountBalancePrefix = "Lock_SubAccount_Balance_";
    private const string FinancialAccountPrefix = "Lock_Account_";
    private const string FinancialSubAccountPrefix = "Lock_SubAccount_";
    private const string BlockChainPaymentAddress = "Lock_HDWallet";
    private const string GameBetPrefix = "GameBet_", GameRewardPrefix = "GameReward_";

    public static string FinancialSubAccountBalance(string userId, long currencyId) => $"{FinancialSubAccountBalancePrefix}{userId}_{currencyId}";
    public static string FinancialAccount(string userId) => $"{FinancialAccountPrefix}{userId}";
    public static string FinancialSubAccount(string userId) => $"{FinancialSubAccountPrefix}{userId}";
    public static string BlockChainHdWallet() => BlockChainPaymentAddress;
    public static string GameBet(string userId) => $"{GameBetPrefix}{userId}";
}