using System.ComponentModel;

namespace BaseDomain.Enums;

public class InformingEnums
{
    public enum InformingSendType
    {
        /// <summary>
        /// SMS
        /// </summary>
        [Description("SMS")]
        Sms,
        /// <summary>
        /// Email
        /// </summary>
        [Description("Email")]
        Email,
        /// <summary>
        /// Notification
        /// </summary>
        [Description("Mobile or Web notification")]
        Notification
    }

    public enum InformingType
    {
        /// <summary>
        /// Reset password
        /// </summary>
        [Description("Reset password")]
        ResetPassword,

        /// <summary>
        /// Verify phone number 
        /// </summary>
        [Description("Verify phone number")]
        VerifyPhoneNumber,

        /// <summary>
        /// Welcome
        /// </summary>
        [Description("Welcome")]
        Welcome,

        /// <summary>
        /// Verify email address 
        /// </summary>
        [Description("Verify email address ")]
        VerifyEmailAddress,

        /// <summary>
        /// Submit order 
        /// </summary>
        [Description("Commit order")]
        CommitOrder,

        /// <summary>
        /// Refund order 
        /// </summary>
        [Description("Refund order")]
        RefundOrder,

        /// <summary>
        /// Refund order 
        /// </summary>
        [Description("Reverse order")]
        ReverseOrder,

        SystemErrorMessage
    }

    public enum PlatformType
    {

    }
}
