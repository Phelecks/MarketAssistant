using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class SendSignUpVerificationCodeMessage : ISendSignUpVerificationCodeMessage
{
    public string UserId { get; }
    /// <summary>
    /// Destination (Email address or Phone number)
    /// </summary>
    public string Destination { get; }

    /// <summary>
    /// verification Code
    /// </summary>
    public string VerificationCode { get; }

    /// <summary>
    /// Send type
    /// </summary>
    public BaseDomain.Enums.InformingEnums.InformingSendType SendType { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="destination"></param>
    /// <param name="verificationCode"></param>
    /// <param name="sendType"></param>
    public SendSignUpVerificationCodeMessage(string userId, string destination, string verificationCode, BaseDomain.Enums.InformingEnums.InformingSendType sendType)
    {
        UserId = userId;
        Destination = destination;
        VerificationCode = verificationCode;
        SendType = sendType;
    }
}