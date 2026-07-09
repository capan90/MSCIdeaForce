using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Problem notu verilerine erişim arayüzü.
/// </summary>
public interface IProblemNoteRepository
{
    /// <summary>
    /// Belirtilen probleme ait tüm notları getirir.
    /// </summary>
    Task<List<ProblemNote>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen ID'ye ait notu getirir.
    /// </summary>
    Task<ProblemNote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir not ekler.
    /// </summary>
    Task AddAsync(ProblemNote note, CancellationToken cancellationToken = default);

    /// <summary>
    /// Not kaydını günceller (soft delete dahil).
    /// </summary>
    Task UpdateAsync(ProblemNote note, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
