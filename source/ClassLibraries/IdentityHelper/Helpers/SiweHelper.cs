namespace IdentityHelper.Helpers;

public static class SiweHelper
{
    public static bool IsThereAnySiweAuthorization(string? authorizationHeader)
    {
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.Contains("Siwe", StringComparison.InvariantCultureIgnoreCase)) 
            return false;
        var siweToken = authorizationHeader.Split(' ').LastOrDefault();
        if (string.IsNullOrEmpty(siweToken)) return false;
        return true;
    }
}