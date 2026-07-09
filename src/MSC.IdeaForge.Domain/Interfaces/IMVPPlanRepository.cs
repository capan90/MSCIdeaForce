using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// MVP Planı verilerine erişim arayüzü.
/// </summary>
public interface IMVPPlanRepository
{
    /// <summary>
    /// Belirtilen probleme ait MVP plan kaydını getirir.
    /// </summary>
    Task<MVPPlan?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir MVP plan kaydı ekler.
    /// </summary>
    Task AddAsync(MVPPlan mvpPlan, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mevcut MVP plan kaydını günceller.
    /// </summary>
    Task UpdateAsync(MVPPlan mvpPlan, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
