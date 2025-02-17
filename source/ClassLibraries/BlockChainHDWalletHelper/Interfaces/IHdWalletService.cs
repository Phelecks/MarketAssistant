using NBitcoin;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace BlockChainHDWalletHelper.Interfaces;

public interface IHdWalletService
{
    Nethereum.HdWallet.Wallet GenerateWallet(string seedPassword, WordCount wordCount);
    Nethereum.HdWallet.Wallet RestoreWallet(List<string> words, string seedPassword);
    Account GetAccount(Nethereum.HdWallet.Wallet wallet, Nethereum.Signer.Chain chain, int index);
    Web3 GetWeb3(Nethereum.HdWallet.Wallet wallet, Nethereum.Signer.Chain chain, int index, string rpcUrl);
    Web3 GetWeb3(Nethereum.HdWallet.Wallet wallet, Nethereum.Signer.Chain chain, string address, string rpcUrl);
}
