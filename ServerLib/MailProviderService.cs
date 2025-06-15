////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using SharedLib;

namespace ServerLib;

/// <summary>
/// Сервис отправки Email
/// </summary>
public class MailProviderService(IOptions<SmtpConfigModel> _config, ILogger<MailProviderService> loggerRepo) : IMailProviderService
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SendEmailAsync(string email, string subject, string message, string mimekit_format = "html", CancellationToken token = default)
    {
        if (email != "*")
        {
            if (!System.Net.Mail.MailAddress.TryCreate(email, out System.Net.Mail.MailAddress? mail) || mail is null)
                return ResponseBaseModel.CreateError("Не корректный Email");

            if (mail.Host == GlobalStaticConstants.FakeHost)
                return ResponseBaseModel.CreateInfo("Заглушка: email host");
        }
        else if (_config.Value.EmailNotificationRecipients.Length == 0)
        {
            string _msg = $"Ошибка отправки технического уведомления: не установлены получатели [{nameof(_config.Value.EmailNotificationRecipients)}]";
            loggerRepo.LogError(_msg);
            return ResponseBaseModel.CreateError(_msg);
        }

        TextFormat format = Enum.Parse<TextFormat>(mimekit_format, true);
        MimeMessage? emailMessage = new();

        emailMessage.From.Add(new MailboxAddress(_config.Value.PublicName, _config.Value.Email));

        if (email == "*")
            foreach (string _mail in _config.Value.EmailNotificationRecipients)
                emailMessage.To.Add(new MailboxAddress(string.Empty, _mail));
        else
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));

        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(format)
        {
            Text = message
        };

        return await SendMessageAsync(emailMessage, token);
    }

    async Task<ResponseBaseModel> SendMessageAsync(MimeMessage emailMessage, CancellationToken token = default)
    {
        using SmtpClient? client = new();
        try
        {
            await client.ConnectAsync(_config.Value.Host, _config.Value.Port, _config.Value.UseSsl, token);
            await client.AuthenticateAsync(_config.Value.Login, _config.Value.Password, token);
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, "error 844D231A-1CD4-470F-AF56-833AAA86BDEA");
            return ResponseBaseModel.CreateError(ex);
        }

        string? res;
        try
        {
            res = await client.SendAsync(emailMessage);
            loggerRepo.LogWarning($"sending email: {res}");
        }
        catch (Exception ex)
        {
            loggerRepo.LogError(ex, "exception sending email");
            return ResponseBaseModel.CreateError(ex);
        }

        await client.DisconnectAsync(true, token);
        return ResponseBaseModel.CreateSuccess(res);
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SendTechnicalEmailNotificationAsync(string message, string mimekit_format = "html", CancellationToken token = default)
    {
        TextFormat format = Enum.Parse<TextFormat>(mimekit_format, true);
        MimeMessage? emailMessage = new();

        emailMessage.From.Add(new MailboxAddress(_config.Value.PublicName, _config.Value.Email));
        emailMessage.To.AddRange(_config.Value.EmailNotificationRecipients.DistinctBy(x => x.ToLower()).Select(x => new MailboxAddress(string.Empty, x)));
        emailMessage.Subject = "ВАЖНОЕ! Серверное уведомление.";
        emailMessage.XPriority = XMessagePriority.High;
        emailMessage.Body = new TextPart(format)
        {
            Text = message
        };

        return await SendMessageAsync(emailMessage, token);
    }
}