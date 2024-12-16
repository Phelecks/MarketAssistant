namespace MassTransitManager.Messages.Interfaces;

public interface ICreateUserMessage
{
    /// <summary>
    /// User identifier
    /// </summary>
    string UserId { get; }

    /// <summary>
    /// phoneNumber
    /// </summary>
    string PhoneNumber { get; }

    /// <summary>
    /// Email address
    /// </summary>
    string EmailAddress { get; }

    /// <summary>
    /// username
    /// </summary>
    string Username { get; }

    string ClientId { get; }

    string? ReferralCode { get; }
}