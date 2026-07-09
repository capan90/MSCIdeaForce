using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Aksiyon planı verilerine erişim arayüzü.
/// </summary>
public interface IActionPlanRepository
{
    /// <summary>
    /// Belirtilen problem ID'sine ait aksiyon planı kaydını getirir.
    /// </summary>
    Task<ActionPlan?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir aksiyon planı kaydı ekler.
    /// </summary>
    Task AddAsync(ActionPlan actionPlan, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aksiyon planı kaydını günceller.
    /// </summary>
    Task UpdateAsync(ActionPlan actionPlan, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
