using BaseDomain.Enums;
using Informing.Application.Interfaces;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace Informing.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly string _displayName, _senderEmailAddress, _senderEmailPassword, _mailServerHost;
        private readonly int _mailServerPort;
        private readonly IApplicationDbContext _context;

        public MailService(IApplicationDbContext context)
        {
            _context = context;
            var baseParameters = _context.baseParameters.ToList();
            _displayName = baseParameters.First(x => x.field == BaseParameterField.InformingMailDisplayName).value;
            _mailServerHost = baseParameters.First(x => x.field == BaseParameterField.InformingMailHost).value;
            _senderEmailPassword = baseParameters.First(x => x.field == BaseParameterField.InformingMailPassword).value;
            _senderEmailAddress = baseParameters.First(x => x.field == BaseParameterField.InformingMailFrom).value;
            _mailServerPort = int.Parse(baseParameters.First(x => x.field == BaseParameterField.InformingMailPort).value);
        }

        public async Task SendEmailAsync(string title, string content, string emailAddress, CancellationToken cancellationToken)
        {
            var email = new MimeMessage();

            //Filling the Information To Send Email
            email.Sender = MailboxAddress.Parse(_senderEmailAddress);
            email.From.Add(MailboxAddress.Parse(_senderEmailAddress));
            email.To.Add(MailboxAddress.Parse(emailAddress));
            email.Subject = title;
            var builder = new BodyBuilder();
            builder.HtmlBody = content;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();

            //Sending The Email
            await smtp.ConnectAsync(_mailServerHost, _mailServerPort, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_senderEmailAddress, _senderEmailPassword, cancellationToken);
            var sendResult = await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
