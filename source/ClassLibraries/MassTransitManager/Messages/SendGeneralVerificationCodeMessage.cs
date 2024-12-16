using MassTransitManager.Messages.Interfaces;

public class SendGeneralVerificationCodeMessage : ISendGeneralVerificationCodeMessage
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

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="verificationCode"></param>
    /// <param name="sendType"></param>
    public SendGeneralVerificationCodeMessage(string userId, string verificationCode, BaseDomain.Enums.InformingEnums.InformingSendType sendType)
    {
        UserId = userId;
        VerificationCode = verificationCode;
        SendType = sendType;
    }
}