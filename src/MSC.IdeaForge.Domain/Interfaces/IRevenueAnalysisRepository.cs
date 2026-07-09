using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Gelir analizi verilerine erişim arayüzü.
/// </summary>
public interface IRevenueAnalysisRepository
{
    /// <summary>
    /// Belirtilen problem ID'sine ait gelir analizi kaydını getirir.
    /// </summary>
    Task<RevenueAnalysis?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir gelir analizi kaydı ekler.
    /// </summary>
    Task AddAsync(RevenueAnalysis revenueAnalysis, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gelir analizi kaydını günceller.
    /// </summary>
    Task UpdateAsync(RevenueAnalysis revenueAnalysis, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
