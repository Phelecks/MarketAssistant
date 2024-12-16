namespace MassTransitManager.Messages.Interfaces;

public interface ISendGeneralVerificationCodeMessage
{
    public string UserId { get; }

    /// <summary>
    /// verification Code
    /// </summary>
    public string VerificationCode { get; }

    /// <summary>
    /// Send type
    /// </summary>
    public BaseDomain.Enums.InformingEnums.InformingSendType SendType { get; }
}