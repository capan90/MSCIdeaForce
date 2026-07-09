using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MSC.IdeaForge.Application.Signals.Services;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Automation.Commands;

public class TriggerRSSScanCommand
{
}

public class TriggerRSSScanHandler
{
    private readonly IRSSFeedRepository _feedRepository;
    private readonly IImportedSignalRepository _signalRepository;
    private readonly RSSImporter _rssImporter;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TriggerRSSScanHandler> _logger;

    public TriggerRSSScanHandler(
        IRSSFeedRepository feedRepository,
        IImportedSignalRepository signalRepository,
        RSSImporter rssImporter,
        IConfiguration configuration,
        ILogger<TriggerRSSScanHandler> logger)
    {
        _feedRepository = feedRepository;
        _signalRepository = signalRepository;
        _rssImporter = rssImporter;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task HandleAsync(TriggerRSSScanCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Manuel veya zamanlanmış RSS taraması başlatılıyor...");

        var dbFeeds = (await _feedRepository.GetAllAsync(cancellationToken)).ToList();

        // DB boşsa seed data ekle
        if (!dbFeeds.Any())
        {
            var configFeeds = _configuration.GetSection("BackgroundJobs:RSSFeeds").Get<List<string>>();
            if (configFeeds != null && configFeeds.Any())
            {
                foreach (var url in configFeeds)
                {
                    var uri = new Uri(url);
                    var name = uri.Host;
                    var newFeed = new RSSFeed(url, name);
                    await _feedRepository.AddAsync(newFeed, cancellationToken);
                    dbFeeds.Add(newFeed);
                }
                await _feedRepository.SaveChangesAsync(cancellationToken);
            }
        }

        if (!dbFeeds.Any())
        {
            return;
        }

        // Mevcut sinyal URL'leri
        var allSignals = await _signalRepository.GetAllAsync(cancellationToken);
        var existingUrls = allSignals
            .Where(s => !string.IsNullOrEmpty(s.SourceUrl))
            .Select(s => s.SourceUrl!)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Aktif feed'leri tara
        foreach (var feed in dbFeeds.Where(f => f.IsActive))
        {
            int newSignalsCount = 0;
            try
            {
                var imported = await _rssImporter.ImportFromRSSAsync(feed.Url);
                foreach (var signal in imported)
                {
                    if (string.IsNullOrEmpty(signal.SourceUrl) || !existingUrls.Contains(signal.SourceUrl))
                    {
                        await _signalRepository.AddAsync(signal, cancellationToken);
                        if (!string.IsNullOrEmpty(signal.SourceUrl))
                        {
                            existingUrls.Add(signal.SourceUrl);
                        }
                        newSignalsCount++;
                    }
                }

                feed.UpdateScanInfo(newSignalsCount);
                await _feedRepository.UpdateAsync(feed, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{FeedName} taranırken hata oluştu: {Message}", feed.Name, ex.Message);
            }
        }

        await _signalRepository.SaveChangesAsync(cancellationToken);
        await _feedRepository.SaveChangesAsync(cancellationToken);
    }
}
