using System.Net.Mail;

namespace Informing.Application.Helpers;

public static class DestinationHelper
{
    public static DestinationType GetDestionationType(string destionation)
    {
        try
        {
            if (!destionation.EndsWith(".") && new MailAddress(destionation).Address == destionation)
            {
                return DestinationType.EmailAddress;
            }
        }
        catch
        {
        }

        if (destionation.StartsWith("00") || destionation.StartsWith('+'))
        {
            return DestinationType.PhoneNumber;
        }

        throw new Exception("Cannot find destionation type (EmailAddress/PhoneNumber).");
    }

    public enum DestinationType
    { EmailAddress, PhoneNumber }
}