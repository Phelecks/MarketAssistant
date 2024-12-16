namespace BlockChainIdentity.Application.Wallet.Queries.GetSiweMessage;

public class SiweMessageDto
{
    public string SiweMessage { get; }

    public SiweMessageDto(string siweMessage)
    {
        SiweMessage = siweMessage;
    }
}
