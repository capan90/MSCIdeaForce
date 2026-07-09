using System;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MSC.IdeaForge.Application.Common.Interfaces;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IEmailSettingRepository _emailSettingRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IEmailSettingRepository emailSettingRepository,
        IConfiguration configuration,
        ILogger<EmailService> logger)
    {
        _emailSettingRepository = emailSettingRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("E-posta gönderiliyor: {To}, Konu: {Subject}", to, subject);

        // Önce DB ayarlarını dene, yoksa fallback olarak config kullan
        string smtpHost = "";
        int smtpPort = 587;
        string smtpUser = "";
        string smtpPassword = "";
        string fromEmail = "";
        string fromName = "MSC IdeaForge";

        try
        {
            var dbSetting = await _emailSettingRepository.GetSettingAsync(cancellationToken);
            if (dbSetting != null && !string.IsNullOrWhiteSpace(dbSetting.SmtpHost))
            {
                smtpHost = dbSetting.SmtpHost;
                smtpPort = dbSetting.SmtpPort;
                smtpUser = dbSetting.SmtpUser;
                smtpPassword = dbSetting.SmtpPassword;
                fromEmail = dbSetting.FromEmail;
                fromName = dbSetting.FromName;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "DB e-posta ayarları okunurken hata oluştu, appsettings.json fallback kullanılacak.");
        }

        if (string.IsNullOrWhiteSpace(smtpHost))
        {
            smtpHost = _configuration["Email:SmtpHost"] ?? "";
            smtpPort = int.TryParse(_configuration["Email:SmtpPort"], out var port) ? port : 587;
            smtpUser = _configuration["Email:SmtpUser"] ?? "";
            smtpPassword = _configuration["Email:SmtpPassword"] ?? "";
            fromEmail = _configuration["Email:FromEmail"] ?? "";
            fromName = _configuration["Email:FromName"] ?? "MSC IdeaForge";
        }

        if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(fromEmail))
        {
            _logger.LogWarning("SMTP sunucusu veya Gönderen e-posta adresi tanımlanmadığı için e-posta gönderimi atlandı.");
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
            if (!string.IsNullOrWhiteSpace(smtpUser))
            {
                await client.AuthenticateAsync(smtpUser, smtpPassword, cancellationToken);
            }
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
            _logger.LogInformation("E-posta başarıyla gönderildi: {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "E-posta gönderilirken hata oluştu: {Message}", ex.Message);
            throw;
        }
    }
}
