using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateTransferWalletMessage
{
    string Address { get; }
    string EncryptedPrivateKey { get; }
    byte[] EncryptedPassword { get; }
    int ChainId { get; }
}