using MSC.IdeaForge.Application.Trends.DTOs;
using MSC.IdeaForge.Domain.Interfaces;

namespace MSC.IdeaForge.Application.Trends.Queries;

/// <summary>
/// Trend analizi kayıtlarını getiren sorgu işleyicisi.
/// </summary>
public class GetTrendAnalysisHandler
{
    private readonly ITrendAnalysisRepository _repository;

    public GetTrendAnalysisHandler(ITrendAnalysisRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Belirtilen trend başlığına ait en güncel analiz kaydını DTO formatında döner.
    /// </summary>
    public async Task<TrendAnalysisDto?> HandleAsync(string trendName, CancellationToken cancellationToken = default)
    {
        var trend = await _repository.GetByTrendNameAsync(trendName, cancellationToken);
        if (trend is null)
        {
            return null;
        }

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

    /// <summary>
    /// Kaydedilmiş tüm trend analizlerini DTO listesi olarak döner.
    /// </summary>
    public async Task<List<TrendAnalysisDto>> HandleAllAsync(CancellationToken cancellationToken = default)
    {
        var trends = await _repository.GetAllAsync(cancellationToken);
        return trends.Select(trend => new TrendAnalysisDto
        {
            Id = trend.Id,
            TrendName = trend.TrendName,
            Summary = trend.Summary,
            Opportunities = trend.Opportunities,
            Actions = trend.Actions,
            AnalyzedAt = trend.AnalyzedAt
        }).ToList();
    }
}
