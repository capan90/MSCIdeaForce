using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Yapay zeka analiz verilerine erişim arayüzü.
/// </summary>
public interface IProblemAnalysisRepository
{
    /// <summary>
    /// Belirtilen problem ID'sine ait yapay zeka analiz kaydını getirir.
    /// </summary>
    Task<ProblemAnalysis?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir yapay zeka analiz kaydı ekler.
    /// </summary>
    Task AddAsync(ProblemAnalysis problemAnalysis, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yapay zeka analiz kaydını günceller.
    /// </summary>
    Task UpdateAsync(ProblemAnalysis problemAnalysis, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
