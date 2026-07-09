using MSC.IdeaForge.Domain.Entities;

namespace MSC.IdeaForge.Domain.Interfaces;

/// <summary>
/// Saha notu verilerine erişim arayüzü.
/// </summary>
public interface IFieldNoteRepository
{
    /// <summary>
    /// Tüm saha notlarını gözlem tarihine göre (yeniden eskiye) getirir.
    /// </summary>
    Task<List<FieldNote>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Yeni bir saha notu ekler.
    /// </summary>
    Task AddAsync(FieldNote fieldNote, CancellationToken cancellationToken = default);

    /// <summary>
    /// Değişiklikleri kaydeder.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
