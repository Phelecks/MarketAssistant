using System.Net.Mail;

namespace Informing.Application.Helpers;

public static class DestinationHelper
{
    public static DestinationType GetDestionationType(string destionation)
    {
        if (destionation.StartsWith("00") || destionation.StartsWith('+'))
                return DestinationType.PhoneNumber;
            
        if (!destionation.EndsWith('.') && new MailAddress(destionation).Address == destionation)
            return DestinationType.EmailAddress;
            
        throw new ArgumentException("Cannot find destionation type (EmailAddress/PhoneNumber).");
    }

    public enum DestinationType
    { EmailAddress, PhoneNumber }
}