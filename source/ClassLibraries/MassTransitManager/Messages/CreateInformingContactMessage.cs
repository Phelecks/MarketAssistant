using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateInformingContactMessage : ICreateInformingContactMessage
{
    public Guid CorrelationId { get; }

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
    /// Constructor
    /// </summary>
    /// <param name="correlationId"></param>
    /// <param name="userId">User identifier</param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="username"></param>
    public CreateInformingContactMessage(Guid correlationId, string userId, string username, string phoneNumber , string emailAddress)
    {
        PhoneNumber = phoneNumber;
        UserId = userId;
        EmailAddress = emailAddress;
        Username = username;
        CorrelationId = correlationId;
    }
}