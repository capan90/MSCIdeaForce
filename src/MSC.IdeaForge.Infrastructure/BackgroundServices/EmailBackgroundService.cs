using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MSC.IdeaForge.Application.Common.Interfaces;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Infrastructure.BackgroundServices;

public class EmailBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmailBackgroundService> _logger;
    private DateTime? _lastSentDate;

    public EmailBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<EmailBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("E-posta Zamanlanmış Görevi Başlatıldı.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.Now;
                // Sabah 09:00 civarı ve bugün henüz gönderilmediyse çalıştır
                if (now.Hour == 9 && (_lastSentDate == null || _lastSentDate.Value.Date != now.Date))
                {
                    _logger.LogInformation("Günlük özet e-postası tetikleniyor...");
                    await SendDailySummaryAsync(stoppingToken);
                    _lastSentDate = now;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Günlük özet gönderimi sırasında hata oluştu: {Message}", ex.Message);
            }

            // Her 30 dakikada bir kontrol et
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private async Task SendDailySummaryAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var settingRepo = scope.ServiceProvider.GetRequiredService<IEmailSettingRepository>();
        var signalRepo = scope.ServiceProvider.GetRequiredService<IImportedSignalRepository>();
        var problemRepo = scope.ServiceProvider.GetRequiredService<IProblemRepository>();
        var opportunityRepo = scope.ServiceProvider.GetRequiredService<IOpportunityRepository>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var settings = await settingRepo.GetSettingAsync(cancellationToken);
        if (settings == null || !settings.DailySummaryEnabled || string.IsNullOrWhiteSpace(settings.ReceiverEmail))
        {
            _logger.LogInformation("Günlük özet e-posta gönderimi devre dışı veya alıcı e-posta adresi ayarlanmamış.");
            return;
        }

        // Dün eklenen sinyal sayısı
        var yesterday = DateTime.UtcNow.AddDays(-1);
        var allSignals = await signalRepo.GetAllAsync(cancellationToken);
        var yesterdaySignalsCount = allSignals.Count(s => s.ImportedAt >= yesterday);

        // Puanlanmamış problem sayısı
        var problems = (await problemRepo.GetAllAsync(cancellationToken)).ToList();
        var opportunities = (await opportunityRepo.GetAllAsync(cancellationToken)).ToList();
        var scoredProblemIds = opportunities
            .Where(o => o.OpportunityScore.TotalScore > 0)
            .Select(o => o.ProblemId)
            .ToHashSet();
        var unscoredProblemsCount = problems.Count(p => !scoredProblemIds.Contains(p.Id));

        // En yüksek skorlu problem
        var topOpportunity = opportunities
            .OrderByDescending(o => o.OpportunityScore.TotalScore)
            .FirstOrDefault();
        string topProblemText = "Bulunmamaktadır.";
        if (topOpportunity != null)
        {
            var topProblem = problems.FirstOrDefault(p => p.Id == topOpportunity.ProblemId);
            if (topProblem != null)
            {
                topProblemText = $"{topProblem.Title} (Skor: {topOpportunity.OpportunityScore.TotalScore}/100)";
            }
        }

        // HTML Gövdesi oluştur
        var subject = $"MSC IdeaForge - Günlük Özet Raporu ({DateTime.Now:dd.MM.yyyy})";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <h2>MSC IdeaForge Günlük Özet Raporu</h2>
                <p>Merhaba,</p>
                <p>İşte son 24 saate ait sistem özeti:</p>
                <table style='border-collapse: collapse; width: 100%; max-width: 600px; margin-top: 15px;'>
                    <tr style='background-color: #f2f2f2;'>
                        <th style='border: 1px solid #ddd; padding: 8px; text-align: left;'>Gösterge</th>
                        <th style='border: 1px solid #ddd; padding: 8px; text-align: left;'>Değer</th>
                    </tr>
                    <tr>
                        <td style='border: 1px solid #ddd; padding: 8px;'>Dün Eklenen Sinyal Sayısı</td>
                        <td style='border: 1px solid #ddd; padding: 8px; font-weight: bold;'>{yesterdaySignalsCount}</td>
                    </tr>
                    <tr>
                        <td style='border: 1px solid #ddd; padding: 8px;'>Puanlanmamış Problem Sayısı</td>
                        <td style='border: 1px solid #ddd; padding: 8px; font-weight: bold; color: {(unscoredProblemsCount > 0 ? "#d9534f" : "#5cb85c")};'>{unscoredProblemsCount}</td>
                    </tr>
                    <tr>
                        <td style='border: 1px solid #ddd; padding: 8px;'>En Yüksek Skorlu Problem</td>
                        <td style='border: 1px solid #ddd; padding: 8px; font-weight: bold; color: #0275d8;'>{topProblemText}</td>
                    </tr>
                </table>
                <br/>
                <p>İyi çalışmalar,<br/><strong>MSC IdeaForge Otomasyon Sistemi</strong></p>
            </body>
            </html>";

        try
        {
            await emailService.SendEmailAsync(settings.ReceiverEmail, subject, body, cancellationToken);
            _logger.LogInformation("Günlük özet e-postası gönderildi: {Receiver}", settings.ReceiverEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Günlük özet e-postası gönderilirken hata oluştu: {Message}", ex.Message);
        }
    }
}
