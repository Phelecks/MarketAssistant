using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class UpdateInformingContactMessage : IUpdateInformingContactMessage
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
    /// Full name
    /// </summary>
    public string Fullname { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="fullname"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="username"></param>
    public UpdateInformingContactMessage(string userId, string username, string fullname, string phoneNumber , string emailAddress)
    {
        PhoneNumber = phoneNumber;
        UserId = userId;
        EmailAddress = emailAddress;
        Username = username;
        Fullname = fullname;
    }
}