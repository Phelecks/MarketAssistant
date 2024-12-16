using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateTransferWalletMessage : ICreateTransferWalletMessage
{
    public CreateTransferWalletMessage(string address, string encryptedPrivateKey, byte[] encryptedPassword, int chainId)
    {
        Address = address;
        EncryptedPrivateKey = encryptedPrivateKey;
        EncryptedPassword = encryptedPassword;
        ChainId = chainId;
    }

    public string Address { get; }
    public string EncryptedPrivateKey { get; }
    public byte[] EncryptedPassword { get; }
    public int ChainId { get; }
}