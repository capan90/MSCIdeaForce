using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Çözüm verilerine erişim arayüzü.
/// </summary>
public interface ISolutionRepository
{
    /// <summary>
    /// Belirtilen probleme ait tüm çözüm önerilerini getirir.
    /// </summary>
    Task<IEnumerable<Solution>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir çözüm kaydı ekler.
    /// </summary>
    Task AddAsync(Solution solution, CancellationToken cancellationToken = default);

    /// <summary>
    /// Mevcut çözüm kaydını günceller.
    /// </summary>
    Task UpdateAsync(Solution solution, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen çözümü veritabanından (soft-delete ile) siler.
    /// </summary>
    Task DeleteAsync(Solution solution, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
