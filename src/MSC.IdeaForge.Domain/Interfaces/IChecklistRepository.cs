using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Kontrol listesi verilerine erişim arayüzü.
/// </summary>
public interface IChecklistRepository
{
    /// <summary>
    /// Belirtilen probleme ait kontrol listesi öğelerini getirir. problemId null ise probleme bağlı olmayanları getirir.
    /// </summary>
    Task<List<ChecklistItem>> GetByProblemIdAsync(Guid? problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen ID'ye ait öğeyi getirir.
    /// </summary>
    Task<ChecklistItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir kontrol listesi öğesi ekler.
    /// </summary>
    Task AddAsync(ChecklistItem item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Öğeyi günceller.
    /// </summary>
    Task UpdateAsync(ChecklistItem item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
