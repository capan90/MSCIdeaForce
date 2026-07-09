using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Rakip analizi verilerine erişim arayüzü.
/// </summary>
public interface ICompetitorAnalysisRepository
{
    /// <summary>
    /// Belirtilen problem ID'sine ait rakip analizi kaydını getirir.
    /// </summary>
    Task<CompetitorAnalysis?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir rakip analizi kaydı ekler.
    /// </summary>
    Task AddAsync(CompetitorAnalysis competitorAnalysis, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rakip analizi kaydını günceller.
    /// </summary>
    Task UpdateAsync(CompetitorAnalysis competitorAnalysis, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
