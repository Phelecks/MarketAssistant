namespace MassTransitManager.Events.Interfaces;

public interface INftUnStakedEvent
{
     Nethereum.Signer.Chain Chain { get; }
    string TransactionHash { get; }
    DateTime DateTime { get; }
    Erc721TransferDto Erc721Transfer { get; }


    record Erc721TransferDto(string From, string To, List<TokenDto> Tokens);
    record TokenDto(int Id, string ContractAddress);
}