using BaseDomain.Enums;

namespace Informing.Application.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string title, string content, string emailAddress, CancellationToken cancellationToken);
    }
}
