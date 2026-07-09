using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Business Model Canvas verilerine erişim arayüzü.
/// </summary>
public interface IBusinessCanvasRepository
{
    /// <summary>
    /// Belirtilen problem ID'sine ait canvas kaydını getirir.
    /// </summary>
    Task<BusinessCanvas?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir canvas kaydı ekler.
    /// </summary>
    Task AddAsync(BusinessCanvas canvas, CancellationToken cancellationToken = default);

    /// <summary>
    /// Canvas kaydını günceller.
    /// </summary>
    Task UpdateAsync(BusinessCanvas canvas, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
