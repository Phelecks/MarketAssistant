using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateUserMessage : ICreateUserMessage
{
    /// <summary>
    /// User identifier
    /// </summary>
    public string UserId { get;  }

    /// <summary>
    /// phoneNumber
    /// </summary>
    public string PhoneNumber { get; }

    /// <summary>
    /// Email address
    /// </summary>
    public string EmailAddress { get; }

    /// <summary>
    /// username
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Client Id
    /// </summary>
    public string ClientId { get; }

    public string? ReferralCode { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="username"></param>
    public CreateUserMessage(string userId, string username, string phoneNumber, string emailAddress, string clientId, string? referralCode = null)
    {
        PhoneNumber = phoneNumber;
        UserId = userId;
        EmailAddress = emailAddress;
        Username = username;
        ClientId = clientId;
        ReferralCode = referralCode;
    }
}