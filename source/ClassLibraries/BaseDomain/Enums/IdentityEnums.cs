using System.ComponentModel;

namespace BaseDomain.Enums;

public class IdentityEnums
{
    public enum TwoFAPurpose
    {
        [Description("User Login")] UserLogin,
        [Description("User Change PassWord")] UserChangePassWord,
        [Description("User Change Email")] UserChangeEmail,
        [Description("User Change Phone")] UserChangePhone,
        [Description("User Change Profile")] UserChangeProfile,
        [Description("User Withdraw")] UserWithdraw
    }

    public enum ReCaptchaPurpose
    {
        [Description("User Login")]
        UserLogin,
        [Description("User Registeration")]
        UserRegistration,
        [Description("User Change PassWord")]
        UserChangePassWord,
        [Description("User Change Email")]
        UserChangeEmail,
        [Description("User Change Phone")]
        UserChangePhone,
        [Description("User Change Profile")]
        UserChangeProfile,
        [Description("User Widthrawal")]
        UserWithdraw,
    }
}