namespace MassTransitManager.Messages.Interfaces;

public interface ICreateCustomerGroupForClientIdMessage
{
    string ClientId { get; }
}