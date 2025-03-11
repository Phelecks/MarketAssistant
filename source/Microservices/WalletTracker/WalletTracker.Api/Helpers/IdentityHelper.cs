using IdentityHelper.Helpers;
using System.Security.Claims;

namespace WalletTracker.Api.Helpers;

/// <summary>
/// Identity service
/// </summary>
public class IdentityHelper : IIdentityHelper
{
    private readonly IHttpContextAccessor _accessor;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="accessor"></param>
    public IdentityHelper(IHttpContextAccessor accessor)
    {
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }

    /// <summary>
    /// Get user identity
    /// </summary>
    /// <returns></returns>
    public string GetUserIdentity()
    {
        var context = _accessor.HttpContext;
        if (context == null) return string.Empty;

        var claim = context.User.FindFirst("id");
        if (claim is null) claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        return claim is null ? string.Empty : claim.Value;
    }

    /// <summary>
    /// Get clientId
    /// </summary>
    /// <returns></returns>
    public string GetClientId()
    {
        var context = _accessor.HttpContext;
        if (context == null) return string.Empty;

        var claim = context.User.FindFirst("client_id");
        return claim is null ? string.Empty : claim.Value;
    }

    /// <summary>
    /// Get username
    /// </summary>
    /// <returns></returns>
    public string GetUserName()
    {
        var context = _accessor.HttpContext;
        if (context == null) return string.Empty;

        var claim = context.User.FindFirst("preferred_username");
        return claim is null ? string.Empty : claim.Value;
    }

    public string GetFullname()
    {
        var context = _accessor.HttpContext;
        if (context == null) return string.Empty;

        var claim = context.User.FindFirst("name");
        return claim is null ? string.Empty : claim.Value;
    }

    public UsernameType GetUsernameType(string username, bool enableNewVerseCode = false, bool ignoreCaseSensitive = true)
    {
        UsernameType type = UsernameType.Username;
        try
        {
            if (!username.EndsWith(".") &&
                new System.Net.Mail.MailAddress(username).Address == username)
                type = UsernameType.Email;
        }
        catch
        {
            // ignored
        }

        if (username.StartsWith("00") || username.StartsWith('+'))
            type = UsernameType.PhoneNumber;

        return type;
    }

    public bool IsAuthorized(string policy, string policyValue = "true")
    {
        var context = _accessor.HttpContext;
        if (context == null) return false;

        return context.User.HasClaim(policy, policyValue);
    }

    public bool IsInRole(string role)
    {
        var context = _accessor.HttpContext;
        if (context == null) return false;

        var claim = context.User.FindFirst("role");
        if (claim is null) claim = context.User.FindFirst(ClaimTypes.Role);
        return claim is not null && claim.Value.ToLower().Equals(role.ToLower());
    }
}