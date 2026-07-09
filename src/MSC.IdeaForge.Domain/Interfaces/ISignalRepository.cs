using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Sinyal verilerine erişim arayüzü.
/// </summary>
public interface ISignalRepository
{
    /// <summary>
    /// Belirtilen probleme ait tüm sinyalleri getirir.
    /// </summary>
    Task<IEnumerable<Signal>> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Toplam sinyal sayısını döner.
    /// </summary>
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir sinyal kaydeder.
    /// </summary>
    Task AddAsync(Signal signal, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri veritabanına kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
