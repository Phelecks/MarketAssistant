using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IDeleteNftTokenMessage : CorrelatedBy<Guid>
{
    string ContractAddress { get; }
}