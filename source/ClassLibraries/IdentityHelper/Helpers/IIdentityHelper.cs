namespace IdentityHelper.Helpers;

/// <summary>
/// Identity service
/// </summary>
public interface IIdentityHelper
{
    /// <summary>
    /// Get user identity
    /// </summary>
    /// <returns></returns>
    string GetUserIdentity();

    /// <summary>
    /// Get clientId
    /// </summary>
    /// <returns></returns>
    string GetClientId();

    /// <summary>
    /// Get username
    /// </summary>
    /// <returns></returns>
    string GetUserName();

    string GetFullname();

    /// <summary>
    /// Username
    /// </summary>
    /// <param name="username"></param>
    /// <param name="enableNewVerseCode"></param>
    /// <param name="ignoreCaseSensitive"></param>
    /// <returns></returns>
    UsernameType GetUsernameType(string username, bool enableNewVerseCode = false, bool ignoreCaseSensitive = true);

    bool IsAuthorized(string policy, string policyValue = "true");
    bool IsInRole(string role);
}

public enum UsernameType
{
    Username,
    Email,
    PhoneNumber
}