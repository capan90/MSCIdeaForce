using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Fırsat verilerine erişim arayüzü.
/// </summary>
public interface IOpportunityRepository
{
    /// <summary>
    /// Belirtilen problem ID'sine ait fırsat değerlendirmesini getirir.
    /// </summary>
    Task<Opportunity?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir fırsat değerlendirmesi ekler.
    /// </summary>
    Task AddAsync(Opportunity opportunity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fırsat değerlendirmesini günceller.
    /// </summary>
    Task UpdateAsync(Opportunity opportunity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
