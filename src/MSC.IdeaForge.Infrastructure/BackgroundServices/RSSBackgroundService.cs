using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MSC.IdeaForge.Application.Automation.Commands;

namespace MSC.IdeaForge.Infrastructure.BackgroundServices;

public class RSSBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RSSBackgroundService> _logger;

    public RSSBackgroundService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<RSSBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RSS Taraması Zamanlanmış Görevi Başlatıldı.");

        var intervalHoursStr = _configuration["BackgroundJobs:RSSIntervalHours"];
        var intervalHours = double.TryParse(intervalHoursStr, out var val) ? val : 6;
        var delayTime = TimeSpan.FromHours(intervalHours);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var triggerHandler = scope.ServiceProvider.GetRequiredService<TriggerRSSScanHandler>();
                await triggerHandler.HandleAsync(new TriggerRSSScanCommand(), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RSS tarama işlemi sırasında bir hata oluştu: {Message}", ex.Message);
            }

            await Task.Delay(delayTime, stoppingToken);
        }
    }
}
