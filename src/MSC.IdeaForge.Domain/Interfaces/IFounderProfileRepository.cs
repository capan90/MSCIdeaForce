using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Kurucu profili verilerine erişim arayüzü.
/// Uygulama tek bir kurucu profili ile çalışır (en güncel kayıt).
/// </summary>
public interface IFounderProfileRepository
{
    /// <summary>
    /// Mevcut (en güncel) kurucu profilini getirir. Kayıt yoksa null döner.
    /// </summary>
    Task<FounderProfile?> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir kurucu profili ekler.
    /// </summary>
    Task AddAsync(FounderProfile founderProfile, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kurucu profilini günceller.
    /// </summary>
    Task UpdateAsync(FounderProfile founderProfile, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
