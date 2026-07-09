using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Araştırma (research) verilerine erişim arayüzü.
/// </summary>
public interface IResearchRepository
{
    /// <summary>
    /// Belirtilen problem ID'sine ait araştırma kaydını getirir.
    /// </summary>
    Task<Research?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir araştırma kaydı ekler.
    /// </summary>
    Task AddAsync(Research research, CancellationToken cancellationToken = default);

    /// <summary>
    /// Araştırma kaydını günceller.
    /// </summary>
    Task UpdateAsync(Research research, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
