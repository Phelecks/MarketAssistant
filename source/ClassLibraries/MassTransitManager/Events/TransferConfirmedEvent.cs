﻿using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TransferConfirmedEvent(Guid correlationId, TransferConfirmedEvent.Transfer transfer) : ITransferConfirmedEvent
{
    public Guid CorrelationId { get;} = correlationId;
    public int Chain { get; } = transfer.Chain;
    public string Hash { get; } = transfer.Hash;
    public string From { get; } = transfer.From;
    public string To { get; } = transfer.To;
    public decimal Value { get; } = transfer.Value;
    public DateTime DateTime { get; } = transfer.DateTime;
    public List<ITransferConfirmedEvent.Erc20Transfer>? Erc20Transfers { get; } = transfer.Erc20Transfers;
    public List<ITransferConfirmedEvent.Erc721Transfer>? Erc721Transfers { get; } = transfer.Erc721Transfers;

    public record Transfer(int Chain, string Hash, string From, string To, decimal Value, DateTime DateTime, List<ITransferConfirmedEvent.Erc20Transfer>? Erc20Transfers, List<ITransferConfirmedEvent.Erc721Transfer>? Erc721Transfers);
}