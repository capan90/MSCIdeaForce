using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Doğrulama (validation) verilerine erişim arayüzü.
/// </summary>
public interface IValidationRepository
{
    /// <summary>
    /// Belirtilen problem ID'sine ait doğrulama kaydını getirir.
    /// </summary>
    Task<Validation?> GetByProblemIdAsync(Guid problemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tüm doğrulama kayıtlarını getirir.
    /// </summary>
    Task<IEnumerable<Validation>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir doğrulama kaydı ekler.
    /// </summary>
    Task AddAsync(Validation validation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Doğrulama kaydını günceller.
    /// </summary>
    Task UpdateAsync(Validation validation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
