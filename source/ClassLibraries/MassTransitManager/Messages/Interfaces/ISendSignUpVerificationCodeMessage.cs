namespace MassTransitManager.Messages.Interfaces;

public interface ISendSignUpVerificationCodeMessage
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
}