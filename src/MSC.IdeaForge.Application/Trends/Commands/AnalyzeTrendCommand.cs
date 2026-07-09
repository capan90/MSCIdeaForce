using MSC.IdeaForge.Application.Trends.DTOs;
using MSC.IdeaForge.Domain.Entities;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Trends.Commands;

/// <summary>
/// Bir trend başlığının yapay zeka ile analiz edilmesi komut nesnesi.
/// </summary>
public class AnalyzeTrendCommand
{
    public string TrendName { get; set; } = string.Empty;
}

/// <summary>
/// Yapay zeka ile trend analizi komutunu işleyen sınıf.
/// </summary>
public class AnalyzeTrendHandler
{
    private readonly ITrendAnalysisRepository _repository;
    private readonly IAIProvider _aiProvider;

    public AnalyzeTrendHandler(ITrendAnalysisRepository repository, IAIProvider aiProvider)
    {
        _repository = repository;
        _aiProvider = aiProvider;
    }

    /// <summary>
    /// Trendi Gemini AI yardımıyla analiz eder ve sonucu kalıcı olarak kaydeder.
    /// </summary>
    public async Task<TrendAnalysisDto> HandleAsync(AnalyzeTrendCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.TrendName))
        {
            throw new ArgumentException("Trend başlığı boş olamaz.", nameof(command.TrendName));
        }

        // Yapay zeka trend analizini tetikliyoruz
        var aiResult = await _aiProvider.AnalyzeTrendAsync(command.TrendName);

        // Mevcut kayıt varsa güncelliyor, yoksa yenisini oluşturuyoruz
        var trend = await _repository.GetByTrendNameAsync(command.TrendName, cancellationToken);
        if (trend is null)
        {
            trend = TrendAnalysis.Create(
                command.TrendName,
                aiResult.Summary,
                aiResult.Opportunities,
                aiResult.Actions
            );
            await _repository.AddAsync(trend, cancellationToken);
        }
        else
        {
            trend.Update(aiResult.Summary, aiResult.Opportunities, aiResult.Actions);
            await _repository.UpdateAsync(trend, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);

        return new TrendAnalysisDto
        {
            Id = trend.Id,
            TrendName = trend.TrendName,
            Summary = trend.Summary,
            Opportunities = trend.Opportunities,
            Actions = trend.Actions,
            AnalyzedAt = trend.AnalyzedAt
        };
    }
}
