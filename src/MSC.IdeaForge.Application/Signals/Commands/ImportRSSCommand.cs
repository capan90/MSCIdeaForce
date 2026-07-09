using System;
using System.Threading;
using System.Threading.Tasks;
using MSC.IdeaForge.Domain.Interfaces;
using MSC.IdeaForge.Application.Signals.Services;

namespace MSC.IdeaForge.Application.Signals.Commands;

/// <summary>
/// RSS URL adresi üzerinden sinyalleri içe aktarma komut nesnesi.
/// </summary>
public class ImportRSSCommand
{
    public string RssUrl { get; set; } = string.Empty;
}

/// <summary>
/// RSS üzerinden sinyalleri içe aktarmayı yöneten komut işleyicisi.
/// </summary>
public class ImportRSSHandler
{
    private readonly RSSImporter _rssImporter;
    private readonly IImportedSignalRepository _repository;

    public ImportRSSHandler(RSSImporter rssImporter, IImportedSignalRepository repository)
    {
        _rssImporter = rssImporter;
        _repository = repository;
    }

    /// <summary>
    /// RSS akışını parse eder ve veritabanında daha önce eklenmemiş olan girdileri kaydeder.
    /// </summary>
    public async Task HandleAsync(ImportRSSCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.RssUrl))
        {
            throw new ArgumentException("RSS URL adresi boş olamaz.", nameof(command));
        }

        var importedSignals = await _rssImporter.ImportFromRSSAsync(command.RssUrl);
        var existingSignals = await _repository.GetAllAsync(cancellationToken);

        // Çift kayıtları (duplicate) önlemek için veritabanında aynı URL'ye sahip kayıtları eliyoruz.
        var existingUrls = existingSignals
            .Where(s => !string.IsNullOrEmpty(s.SourceUrl))
            .Select(s => s.SourceUrl!)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var signal in importedSignals)
        {
            if (string.IsNullOrEmpty(signal.SourceUrl) || !existingUrls.Contains(signal.SourceUrl))
            {
                await _repository.AddAsync(signal, cancellationToken);
            }
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }
}
