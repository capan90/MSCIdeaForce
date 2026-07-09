using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Trend analiz verilerine erişim arayüzü.
/// </summary>
public interface ITrendAnalysisRepository
{
    /// <summary>
    /// Belirtilen trend başlığına ait en güncel analiz kaydını getirir.
    /// </summary>
    Task<TrendAnalysis?> GetByTrendNameAsync(string trendName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kaydedilmiş tüm trend analizlerini getirir.
    /// </summary>
    Task<List<TrendAnalysis>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir trend analizi kaydı ekler.
    /// </summary>
    Task AddAsync(TrendAnalysis trendAnalysis, CancellationToken cancellationToken = default);

    /// <summary>
    /// Trend analizi kaydını günceller.
    /// </summary>
    Task UpdateAsync(TrendAnalysis trendAnalysis, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
