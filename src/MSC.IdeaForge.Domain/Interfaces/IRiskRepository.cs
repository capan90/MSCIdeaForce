using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Risk verilerine erişim arayüzü.
/// </summary>
public interface IRiskRepository
{
    /// <summary>
    /// Belirtilen probleme ait tüm riskleri getirir.
    /// </summary>
    Task<List<RiskItem>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir risk kaydı ekler.
    /// </summary>
    Task AddAsync(RiskItem risk, CancellationToken cancellationToken = default);

    /// <summary>
    /// Risk kaydını günceller (soft delete dahil).
    /// </summary>
    Task UpdateAsync(RiskItem risk, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
