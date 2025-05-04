namespace MassTransitManager.Events.Interfaces;

public interface INftTradedEvent
{
    Nethereum.Signer.Chain Chain { get; }
    string TransactionHash { get; }
    DateTime DateTime { get; }
    Erc721TransferDto Erc721Transfer { get; }
    List<Erc20TransferDto> Erc20Transfers { get; }

    record Erc721TransferDto(string From, string To, List<TokenDto> Tokens);
    record TokenDto(int Id, string ContractAddress);
    
    record Erc20TransferDto(string From, string To, decimal Value, string? ContractAddress);
}